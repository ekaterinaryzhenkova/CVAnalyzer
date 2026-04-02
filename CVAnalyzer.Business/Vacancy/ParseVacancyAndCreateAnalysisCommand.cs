using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CVAnalyzer.Business.Vacancy
{
    public class ParseVacancyAndCreateAnalysisCommand(
        IHhClient hhClient,
        IPromptService promptService,
        IAnalysisResponseMapper responseMapper,
        IAiClient aiClient,
        IDbAnalysisMapper dbAnalysisMapper,
        IAnalysisRepository analysisRepository,
        ICvRepository cvRepository,
        ILogger<ParseVacancyAndCreateAnalysisCommand> logger)
        : IParseVacancyAndCreateAnalysisCommand
    {
        public async Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(VacancyRequest request)
        {
            if (request.Link is null)
            {
                return await GetAnalysis(request.CvId);
            }
            
            //https://spb.hh.ru/vacancy/130452842?hhtmFromLabel=suitable_vacancies_sidebar&hhtmFrom=vacancy
            string[] collection = request.Link.Split('/');
            string vacancyId = collection[^1];
            
            int length = vacancyId.IndexOf('?');
            if (length == -1)
                length = vacancyId.Length;
            
            vacancyId = vacancyId.Substring(0, length);

            if (!int.TryParse(vacancyId, out _))
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "The vacancy link is incorrect.",
                    ResultStatus.BadRequest);
            }

            try
            {
                var response = await hhClient.ParseVacancyAsync(vacancyId);

                if (response.StatusCode is HttpStatusCode.OK)
                {
                    logger.LogInformation("Vacancy was parsed.");
                    return await GetAnalysis(request.CvId, response.Content);
                }

                logger.LogWarning("Vacancy wasn't parsed. Try to analyze Cv");
                return await GetAnalysis(request.CvId);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex.Message);
                return new OperationResultResponse<AnalysisResponse>(
                    "Error accessing an external server.",
                    ResultStatus.ExternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new OperationResultResponse<AnalysisResponse>(
                    "Unexpected error.",
                    ResultStatus.InternalServerError);
            }
        }

        private async Task<OperationResultResponse<AnalysisResponse>> GetAnalysis(Guid cvId, string? vacancyText = null)
        {
            var cv = await cvRepository.GetAsync(cvId);

            if (cv is null)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "Cv is not found",
                    ResultStatus.NotFound); 
            }

            string prompt;
            
            if (vacancyText is null)
            {
                string template = await promptService.GetAsync("CvAnalysis");
                prompt = string.Format(template, cv.ParsedText);
            }
            else
            {
                string template = await promptService.GetAsync("CvAndVacancyAnalysis");
                prompt = string.Format(template, cv.ParsedText, vacancyText);
            }
            
            try
            {
                var response = await aiClient.SendMessageAsync(prompt);

                if (response.StatusCode is not HttpStatusCode.OK)
                {
                    return await OperationResultResponseHelper.HttpToOperationResultAsync<AnalysisResponse>(response.StatusCode);
                }

                
                AnalysisResponse result = responseMapper.Map(response.Content);
                await analysisRepository.CreateAsync(dbAnalysisMapper.Map(result, cvId));
            
                return new OperationResultResponse<AnalysisResponse>(result);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex.Message);
                return new OperationResultResponse<AnalysisResponse>(
                    ex.Message,
                    ResultStatus.ExternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new OperationResultResponse<AnalysisResponse>(
                    "Unexpected error.",
                    ResultStatus.InternalServerError);
            }
        }
    }
}