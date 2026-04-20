using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class UserRepository(CVAnalyzerContext dbContext) : IUserRepository
    {
        public async Task<Guid> CreateAsync(DbUser user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            
            return user.Id;
        }
        
        public async Task<DbUser?> GetByLoginAsync(string login)
        {
            return await dbContext.Users
                .Include(x => x.UsersCredentials)
                .FirstOrDefaultAsync(x => x.UsersCredentials.Login == login);
        }

        public async Task<bool> IsLoginAlreadyExistsAsync(string login)
        {
            return await dbContext.Users
                .Include(x => x.UsersCredentials)
                .AnyAsync(x => x.UsersCredentials.Login == login);
        }
        
        public async Task<UserInfo?> GetUserInfoAsync(Guid id)
        {
            return await dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new UserInfo(u.FirstName, u.LastName))
                .FirstOrDefaultAsync();
        }
    }
}