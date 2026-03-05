using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> CreateAsync(DbUser user);

        Task<DbUser?> GetByLoginAsync(string login);
        
        
        
        Task<bool> IsLoginAlreadyExistsAsync(string login);
    }
}