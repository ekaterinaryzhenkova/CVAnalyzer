using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class CvRepository(
        CVAnalyzerContext dbContext) 
        : ICvRepository
    {
        public async Task<DbCV> CreateAsync(DbCV cv)
        {
            dbContext.CVs.Add(cv);
            await dbContext.SaveChangesAsync();

            return cv;
        }
        
        public async Task<DbCV?> GetAsync(Guid id)
        {
            return await dbContext.CVs
                .AsNoTracking()
                .FirstOrDefaultAsync(cv => cv.Id == id);
        }
    }
}