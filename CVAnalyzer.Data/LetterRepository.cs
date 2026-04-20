using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Repositories.Interfaces;

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
    }
}