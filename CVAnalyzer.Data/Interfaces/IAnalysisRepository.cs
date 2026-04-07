using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IAnalysisRepository
    {
        Task<DbAnalysis> CreateAsync(DbAnalysis analysis);
        
        Task<DbAnalysis?> GetAsync(Guid analysisId);

        Task<AnalysisResponse?> GetAnalasisResponseAsync(Guid analysisId);

        Task<DbAnalysis?> GetNewAnalysisAsync();

        Task<int> UpdateAsync(
            Guid analysisId,
            AnalysisStatus status,
            string structure,
            string technologies,
            string relevance,
            string another,
            string? vacancyComparison = null,
            string? vacancyLink = null);

        Task<int> UpdateAsync(
            Guid analysisId,
            AnalysisStatus status);
    }
}