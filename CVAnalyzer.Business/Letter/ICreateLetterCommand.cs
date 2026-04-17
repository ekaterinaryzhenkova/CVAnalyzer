using CVAnalyzer.Models.OperationResultResponse;

namespace CVAnalyzer.Business.Letter
{
    public interface ICreateLetterCommand
    {
        Task<OperationResultResponse<string>> ExecuteAsync(Guid analysisId);
    }
}