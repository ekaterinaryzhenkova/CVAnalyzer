using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class LetterRepository(CVAnalyzerContext dbContext) : ILetterRepository
    {
        public async Task<Guid> CreateAsync(DbLetter letter)
        {
            dbContext.Letters.Add(letter);
            await dbContext.SaveChangesAsync();
            
            return letter.Id;
        }
        
        public async Task<DbLetter?> GetAsync(Guid id)
        {
            return await dbContext.Letters
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        
        public async Task<int> UpdateAsync(Guid letterId, LetterStatus status)
        {
            var letter = await dbContext.Letters.FirstAsync(l => l.Id == letterId);

            letter.Status = status;

            return await dbContext.SaveChangesAsync();
        }
        
        public async Task<int> UpdateAsync(Guid letterId, string text, LetterStatus status)
        {
            var letter = await dbContext.Letters.FirstAsync(l => l.Id == letterId);

            letter.Text = text;
            letter.Status = status;

            return await dbContext.SaveChangesAsync();
        }
    }
}