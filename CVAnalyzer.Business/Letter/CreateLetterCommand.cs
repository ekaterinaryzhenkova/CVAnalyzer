using CVAnalyzer.Business.Letter.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.RabbitMq;
using CVAnalyzer.Repositories.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.Letter
{
    public class CreateLetterCommand(
        ILetterRepository letterRepository,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateLetterCommand> logger)
        : ICreateLetterCommand
    {
        public async Task<OperationResultResponse<Guid>> ExecuteAsync(Guid analysisId)
        {
            var letter = new DbLetter
            {
                Id = Guid.NewGuid(),
                AnalysisId = analysisId,
                Status = LetterStatus.Created
            };
            
            try
            {
                await letterRepository.CreateAsync(letter);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while saving letter");
                return new OperationResultResponse<Guid>(
                    "Error while saving letter",
                    ResultStatus.InternalServerError);
            }
            
            await publishEndpoint.Publish(new CreateLetterMessage(letter.Id));
            
            return new OperationResultResponse<Guid>(letter.Id);
        }
    }
}