using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class AnalysisRepository(
        CVAnalyzerContext context,
        IAnalysisResponseMapper mapper) 
        : IAnalysisRepository
    {
        public async Task<DbAnalysis> CreateAsync(DbAnalysis analysis)
        {
            context.Analyses.Add(analysis);
            await context.SaveChangesAsync();
            
            return analysis;
        }

        public async Task<DbAnalysis?> GetAsync(Guid analysisId)
        {
            return await context.Analyses
                .FirstOrDefaultAsync(a => a.Id == analysisId);
        }

        public async Task<AnalysisResponse?> GetAnalysisResponseAsync(Guid analysisId)
        {
            return await context.Analyses
                .Where(a => a.Id == analysisId)
                .Select(a => mapper.Map(a))
                .FirstOrDefaultAsync();
        }
        
        public async Task<DbAnalysis?> GetNewAnalysisAsync()
        {
            return await context.Analyses
                .Where(r => r.Status == AnalysisStatus.Created)
                .OrderBy(r=> r.CreatedAt)
                .FirstOrDefaultAsync();
        }
        
        public async Task<int> UpdateAsync(
            Guid analysisId,
            AnalysisStatus status,
            string structure,
            string technologies,
            string relevance,
            string another,
            string? vacancyText = null,
            string? vacancyComparison = null)
        {
            var analysis = await context.Analyses.FirstAsync(r => r.Id == analysisId);

            analysis.Status = status;

            analysis.Structure = structure;
            analysis.Technologies = technologies;
            analysis.Relevance = relevance;
            analysis.Another = another;
            
            if (vacancyText is not null)
                analysis.VacancyText = vacancyText;
            
            if (vacancyComparison is not null)
                analysis.VacancyComparison = vacancyComparison;

            return await context.SaveChangesAsync();
        }
        
        public async Task<int> UpdateAsync(
            Guid analysisId,
            AnalysisStatus status)
        {
            var analysis = await context.Analyses.FirstAsync(a => a.Id == analysisId);

            analysis.Status = status;

            return await context.SaveChangesAsync();
        }
        
        public async Task<string?> GetVacancyTextAsync(Guid analysisId)
        {
            return await context.Analyses
                .Where(a => a.Id == analysisId)
                .Select(a => a.VacancyText)
                .FirstAsync();
        }
        
        public async Task<(string? cvText, string? vacancyText)> GetVacancyAndCvTextAsync(Guid analysisId)
        {
            var result = await context.Analyses
                .Where(a => a.Id == analysisId)
                .Select(a => new
                {
                    VacancyText = a.VacancyText,
                    CvText = a.CV.ParsedText
                })
                .FirstOrDefaultAsync();

            return (result?.CvText, result?.VacancyText);
        }
    }
}