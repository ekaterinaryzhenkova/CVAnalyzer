using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Repositories.Interfaces;

namespace CVAnalyzer.Repositories
{
    public class AnalysisRepository(
        CVAnalyzerContext dbContext) 
        : IAnalysisRepository
    {
        public async Task<DbAnalysis> CreateAsync(DbAnalysis analysis)
        {
            dbContext.Analyses.Add(analysis);
            await dbContext.SaveChangesAsync();
            
            return analysis;
        }
    }
}