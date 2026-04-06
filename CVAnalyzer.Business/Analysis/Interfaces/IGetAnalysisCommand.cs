using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.Analysis.Interfaces
{
    public interface IGetAnalysisCommand
    {
        Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(Guid analysisId);
    }
}