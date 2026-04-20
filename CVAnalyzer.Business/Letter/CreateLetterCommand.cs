using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CVAnalyzer.Business.Letter
{
    public class CreateLetterCommand(
        IPromptService promptService,
        IAnalysisRepository analysisRepository,
        ILetterRepository letterRepository,
        IAiClient aiClient,
        ILogger<CreateLetterCommand> logger)
        : ICreateLetterCommand
    {
        public async Task<OperationResultResponse<string>> ExecuteAsync(Guid analysisId)
        {
            (string? cvText, string? vacancyText) = await analysisRepository.GetVacancyAndCvTextAsync(analysisId);

            if (string.IsNullOrEmpty(cvText) || string.IsNullOrEmpty(vacancyText))
            {
                return new OperationResultResponse<string>(
                    "Cv or vacancy not found",
                    ResultStatus.NotFound);
            }
            
            string template = await promptService.GetAsync("LetterCreating");
            string prompt = string.Format(template, cvText, vacancyText);
            
            //TODO: add cache later?

            try
            {
                string letter = await aiClient.CreateLetterAsync(prompt);
                var dbLetter = new DbLetter()
                {
                    Id = Guid.NewGuid(),
                    AnalysisId = analysisId,
                    Text = letter
                };
                await letterRepository.CreateAsync(dbLetter);
                
                return new OperationResultResponse<string>(letter);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "AI service error");

                return new OperationResultResponse<string>(
                    "AI service error",
                    ResultStatus.ExternalServerError);
            }
            catch (TaskCanceledException ex)
            {
                logger.LogError(ex, "AI timeout");

                return new OperationResultResponse<string>(
                    "AI request timeout",
                    ResultStatus.ExternalServerError);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "AI response parsing error");

                return new OperationResultResponse<string>(
                    "Invalid response from AI",
                    ResultStatus.ExternalServerError);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error");

                return new OperationResultResponse<string>(
                    "Unexpected error",
                    ResultStatus.InternalServerError);
            }
        }
    }
}