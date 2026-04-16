using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.User.Interfaces
{
    public interface IGetUserAnalysisCommand
    {
        Task<OperationResultResponse<List<AnalysisResponse>>> ExecuteAsync(Guid userId);
    }
}