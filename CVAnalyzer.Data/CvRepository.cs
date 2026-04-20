using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class CvRepository(
        CVAnalyzerContext dbContext,
        IAnalysisResponseMapper analysisMapper) 
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
        
        public async Task<List<AnalysisResponse>> GetAnalysisAsync(Guid userId)
        {
            return await dbContext.CVs
                .Include(cv => cv.Analysis)
                .ThenInclude(a => a.Letter)
                .Where(cv => cv.UserId == userId)
                .SelectMany(cv => cv.Analysis)
                .Select(a => analysisMapper.Map(a))
                .ToListAsync();
        }
        
        public async Task<List<ComplexAnalysisResponse>> GetComplexAnalysisAsync(Guid userId)
        {
            return await dbContext.CVs
                .Include(cv => cv.Analysis)
                .ThenInclude(a => a.Letter)
                .Where(cv => cv.UserId == userId)
                .SelectMany(cv => cv.Analysis)
                .Select(a => analysisMapper.ComplexAnalysisMap(a))
                .ToListAsync();
        }
    }
}