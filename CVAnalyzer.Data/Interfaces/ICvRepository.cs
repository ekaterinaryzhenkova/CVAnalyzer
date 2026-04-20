using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface ICvRepository
    {
        Task<DbCV> CreateAsync(DbCV cv);

        Task<DbCV?> GetAsync(Guid id);

        Task<List<AnalysisResponse>> GetAnalysisAsync(Guid userId);

        Task<List<ComplexAnalysisResponse>> GetComplexAnalysisAsync(Guid userId);
    }
}