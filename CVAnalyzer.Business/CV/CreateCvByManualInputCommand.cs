using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CVAnalyzer.Business.CV
{
    public class CreateCvByManualInputCommand(
        ICVRepository repository,
        IAnalysisResponseMapper responseMapper,
        IDbAnalysisMapper dbAnalysisMapper,
        IAnalysisRepository analysisRepository,
        IPromptService promptService,
        IAiClient aiClient,
        ILogger<CreateCvByManualInputCommand> logger) 
        : ICreateCvByManualInputCommand
    {
        public async Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(ManualCvRequest manualCvRequest)
        {
            string cv = "ФИО: " + manualCvRequest.FullName + "\n" +
                        "Позиция: " + manualCvRequest.Position + "\n" +
                        "Навыки: " + manualCvRequest.Skills + "\n" +
                        "Опыт: " + manualCvRequest.Experience + "\n" +
                        "Образование: " + manualCvRequest.Education + "\n" +
                        "О себе: " + manualCvRequest.AboutYourself;
                        
            string? template = await promptService.GetAsync("CvAnalysis");

            if (template is null)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "Prompt is not found",
                    ResultStatus.InternalServerError);
            }
            
            string prompt = string.Format(template, cv);
            
            try
            {
                var response = await aiClient.SendMessageAsync(prompt);

                if (response.StatusCode is not HttpStatusCode.OK)
                {
                    return await OperationResultResponseHelper.HttpToOperationResultAsync<AnalysisResponse>(response.StatusCode);
                }

                AnalysisResponse result = responseMapper.Map(response.Content);
                await analysisRepository.CreateAsync(dbAnalysisMapper.Map(result));
            
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