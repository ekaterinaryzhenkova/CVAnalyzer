using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;

namespace CVAnalyzer.Business.Letter
{
    public class CreateLetterCommand(
        IPromptService promptService,
        IAnalysisRepository analysisRepository,
        IAiClient aiClient)
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
            
            //TODO: добавить кэш позже?
            return new OperationResultResponse<string>(await aiClient.CreateLetterAsync(prompt));
        }
    }
}