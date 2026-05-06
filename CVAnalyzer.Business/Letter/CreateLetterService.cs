using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.Letter.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.Letter
{
    public class CreateLetterService(
        IPromptService promptService,
        IAiClient aiClient,
        IAnalysisRepository analysisRepository,
        ILetterRepository letterRepository,
        ILogger<ICreateAnalysisService> logger)
        : ICreateLetterService
    {
        public async Task ExecuteAsync(Guid letterId)
        {
            logger.LogInformation("Letter creating started.");
            
            var letter = await letterRepository.GetAsync(letterId);

            if (letter is null)
            {
                logger.LogInformation("No analyses was found");
                return;
            }

            if (letter.Status == LetterStatus.Done)
            {
                logger.LogInformation("Letter is already created.");
                return;
            }

            await letterRepository.UpdateAsync(
                letterId,
                LetterStatus.Processing);

            try
            {
                var (cvText, vacancyText) =
                    await analysisRepository.GetVacancyAndCvTextAsync(letter.AnalysisId);

                if (string.IsNullOrEmpty(cvText) ||
                    string.IsNullOrEmpty(vacancyText))
                {
                    await letterRepository.UpdateAsync(
                        letterId,
                        LetterStatus.Failed);

                    return;
                }

                string template =
                    await promptService.GetAsync("LetterCreating");

                string prompt =
                    string.Format(template, cvText, vacancyText);

                string generatedLetter =
                    await aiClient.CreateLetterAsync(prompt); //тут момент с очередью

                await letterRepository.UpdateAsync(
                    letterId,
                    generatedLetter,
                    LetterStatus.Done);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Letter generation failed");

                await letterRepository.UpdateAsync(
                    letterId,
                    LetterStatus.Failed);

                throw;
            }
        }
    }
}