using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Repositories.Interfaces;

namespace CVAnalyzer.Repositories
{
    public class CVRepository(
        CVAnalyzerContext dbContext) 
        : ICVRepository
    {
        public async Task<DbCV> CreateAsync(ManualCvRequest cvRequest)
        {
            DbCV dbCv = new DbCV
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("7f679a99-6b3e-4007-8b8b-5d86bfa73902"),
                ParsedText = cvRequest.Education + cvRequest.Experience + cvRequest.Skills
            };

            dbContext.CVs.Add(dbCv);

            await dbContext.SaveChangesAsync();

            return dbCv;
        }
        
        public async Task<DbCV> CreateAsync(string cv)
        {
            DbCV dbCv = new DbCV
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("7f679a99-6b3e-4007-8b8b-5d86bfa73902"),
                ParsedText = cv
            };

            dbContext.CVs.Add(dbCv);

            await dbContext.SaveChangesAsync();

            return dbCv;
        }
    }
}