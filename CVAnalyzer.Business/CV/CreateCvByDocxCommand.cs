using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.Interfaces;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CVAnalyzer.Business.CV
{
    public class CreateCvByDocxCommand(
        IAnalysisResponseMapper responseMapper,
        IDbAnalysisMapper dbAnalysisMapper,
        IAnalysisRepository analysisRepository,
        IPromptRepository promptRepository,
        IAiClient aiClient) 
        : ICreateCVbyDocxCommand
    {
        public async Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(IFormFile uploadedFile)
        {
            using var memoryStream = new MemoryStream();
            await uploadedFile.CopyToAsync(memoryStream);
            
            memoryStream.Position = 0;

            using var wordDoc = WordprocessingDocument.Open(memoryStream, false);
            var body = wordDoc.MainDocumentPart.Document.Body;

            string cv =  body.InnerText;
            
            if (cv.Length == 0)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "The uploaded file is empty.",
                    ResultStatus.BadRequest);
            }
            
            string? template = await promptRepository.GetAsync("CvAnalysis");

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
                return new OperationResultResponse<AnalysisResponse>(
                    ex.Message,
                    ResultStatus.ExternalServerError);
            }
            catch (Exception)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "Unexpected error.",
                    ResultStatus.InternalServerError);
            }
        }
    }
}