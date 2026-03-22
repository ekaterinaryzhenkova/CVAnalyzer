using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.helpers;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CVAnalyzer.Business.CV
{
    public class CreateCvByDocxCommand(
        IAnalysisResponseMapper responseMapper,
        IDbAnalysisMapper dbAnalysisMapper,
        IAnalysisRepository analysisRepository,
        IPromptService promptService,
        IParseCvHelper parseHelper,
        IAiClient aiClient,
        ILogger<CreateCvByDocxCommand> logger) 
        : ICreateCvByDocxCommand
    {
        public async Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(IFormFile uploadedFile)
        {
            string cv =  await parseHelper.ParseCvByDocx(uploadedFile);
            
            if (cv.Length == 0)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "The uploaded file is empty.",
                    ResultStatus.BadRequest);
            }
            
            string? template = await promptService.GetAsync("CvAnalysis");

            if (template is null)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "Prompt is not found",
                    ResultStatus.NotFound);
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