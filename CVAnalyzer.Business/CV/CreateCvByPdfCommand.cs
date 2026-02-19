using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace CVAnalyzer.Business.CV;

public class CreateCvByPdfCommand(
    IAnalysisResponseMapper responseMapper,
    IDbAnalysisMapper dbAnalysisMapper,
    IAnalysisRepository analysisRepository,
    IPromptService promptService,
    IAiClient aiClient,
    ILogger<CreateCvByPdfCommand> logger) 
    : ICreateCbByPdfCommand
{
    public async Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(IFormFile uploadedFile)
    {
        string cv = (await ExtractTextAsync(uploadedFile)).ParsedText;
        
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
    
    private async Task<CvAfterPdf> ExtractTextAsync(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;
        
        var text = new StringBuilder();

        using (PdfDocument document = PdfDocument.Open(ms))
        {
            foreach (Page page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }
        }

        var result = new CvAfterPdf
        {
            ParsedText = text.ToString(),
            Education = ExtractSection(text.ToString(), "Образование", new[] { "Навыки", "Опыт работы" }),
            Skills = ExtractSection(text.ToString(), "Навыки", new[] { "Образование", "Опыт работы" }),
            Experience = ExtractSection(text.ToString(), "Опыт работы", new[] { "Навыки", "Образование" })
        };
        
        return result;
    }
    
    private string ExtractSection(string fullText, string sectionName, string[] otherSectionNames)
    {
        int startIndex = fullText.IndexOf(sectionName, StringComparison.OrdinalIgnoreCase);
        if (startIndex == -1)
            return null;

        startIndex += sectionName.Length;
        
        int endIndex = fullText.Length;
        foreach (var other in otherSectionNames)
        {
            int idx = fullText.IndexOf(other, startIndex, StringComparison.OrdinalIgnoreCase);
            if (idx != -1 && idx < endIndex)
                endIndex = idx;
        }
        
        var result = fullText.Substring(startIndex, endIndex - startIndex);

        return result.Trim();
    }
}