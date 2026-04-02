using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.Vacancy.Interfaces
{
    public interface IParseVacancyAndCreateAnalysisCommand
    {
        Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(VacancyRequest request);
    }
}