using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> CreateAsync(DbUser user);

        Task<DbUser?> GetByLoginAsync(string login);

        Task<UserInfo?> GetUserInfoAsync(Guid id);
        
        Task<bool> IsLoginAlreadyExistsAsync(string login);
    }
}