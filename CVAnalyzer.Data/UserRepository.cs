using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class UserRepository(CVAnalyzerContext dbContext) : IUserRepository
    {
        public async Task<Guid> Create(DbUser user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            
            return user.Id;
        }
        
        public async Task<bool> GetByLoginAsync(string login)
        {
            return await dbContext.Users
                .Include(x => x.UsersCredentials)
                .AnyAsync(x => x.UsersCredentials.Login == login);
        }
    }
}