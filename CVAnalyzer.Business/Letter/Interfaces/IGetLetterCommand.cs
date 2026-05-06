using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.Letter.Interfaces
{
    public interface IGetLetterCommand
    {
        Task<OperationResultResponse<LetterResponse>> ExecuteAsync(Guid letterId);
    }
}