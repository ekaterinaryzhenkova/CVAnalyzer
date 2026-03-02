using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Create(DbUser user);

        Task<bool> GetByLoginAsync(string login);
    }
}