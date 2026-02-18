using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IAnalysisRepository
    {
        Task<DbAnalysis> CreateAsync(DbAnalysis analysis);
    }
}