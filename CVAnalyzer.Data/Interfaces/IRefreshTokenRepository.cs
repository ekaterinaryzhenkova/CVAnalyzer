using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> CreateAsync(DbRefreshToken token);

        Task<bool> RemoveAsync(List<DbRefreshToken> tokens);

        Task<DbRefreshToken?> GetAsyns(string requestToken);

        Task<List<DbRefreshToken>> GetExpiredAsync();
    }
}