using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;

namespace CVAnalyzer.Business.Analysis.Interfaces
{
    public interface IStartAnalysisCommand
    {
       Task<OperationResultResponse<Guid>> ExecuteAsync(VacancyRequest request);
    }
}