using CVAnalyzer.Models.OperationResultResponse;

namespace CVAnalyzer.Business.Letter.Interfaces
{
    public interface ICreateLetterCommand
    {
        Task<OperationResultResponse<Guid>> ExecuteAsync(Guid analysisId);
    }
}