using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class PromptRepository(
        CVAnalyzerContext dbContext) 
        : IPromptRepository
    {
        public async Task<string> GetAsync(string name)
        {
            return await dbContext.Prompts
                .AsNoTracking()
                .Where(p => p.Name == name & p.IsActive)
                .Select(p => p.Content)
                .FirstAsync();
        }
    }
}