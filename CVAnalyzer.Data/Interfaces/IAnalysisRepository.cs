using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IAnalysisRepository
    {
        Task<DbAnalysis> CreateAsync(DbAnalysis analysis);
        
        Task<DbAnalysis?> GetAsync(Guid analysisId);

        Task<AnalysisResponse?> GetAnalysisResponseAsync(Guid analysisId);

        Task<DbAnalysis?> GetNewAnalysisAsync();

        Task<int> UpdateAsync(
            Guid analysisId,
            AnalysisStatus status,
            string structure,
            string technologies,
            string relevance,
            string another,
            string? vacancyText = null,
            string? vacancyComparison = null);

        Task<int> UpdateAsync(
            Guid analysisId,
            AnalysisStatus status);

        Task<(string? cvText, string? vacancyText)> GetVacancyAndCvTextAsync(Guid analysisId);
    }
}