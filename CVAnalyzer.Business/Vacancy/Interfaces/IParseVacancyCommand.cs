using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;

namespace CVAnalyzer.Business.Vacancy.Interfaces
{
    public interface IParseVacancyCommand
    {
        Task<OperationResultResponse<string>> ExecuteAsync(VacancyRequest vacancy);
    }
}