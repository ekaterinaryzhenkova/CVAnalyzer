using CVAnalyzer.Business.Letter.Interfaces;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.Letter
{
    public class GetLetterCommand(
        ILetterRepository repository,
        ILetterResponseMapper mapper,
        ILogger<GetLetterCommand> logger) 
        : IGetLetterCommand
    {
        public async Task<OperationResultResponse<LetterResponse>> ExecuteAsync(Guid letterId)
        {
            var dbLetter = await repository.GetAsync(letterId);

            if (dbLetter is null)
            {
                logger.LogInformation("Letter with {Id} wasn't found", letterId);
                return new OperationResultResponse<LetterResponse>(
                    "No analysis was found",
                    ResultStatus.NotFound);
            }

            if (dbLetter.Status == LetterStatus.Processing || dbLetter.Status == LetterStatus.Created)
            {
                return new OperationResultResponse<LetterResponse>(
                    "Letter in processing",
                    ResultStatus.InProgress);
            }

            if (dbLetter.Status == LetterStatus.Failed)
            {
                logger.LogInformation("Creation of letter with {Id} was failed", letterId);
                return new OperationResultResponse<LetterResponse>(
                    "Letter creating is failed",
                    ResultStatus.Ok);
            }

            return new OperationResultResponse<LetterResponse>(mapper.Map(dbLetter));
        }
    }
}