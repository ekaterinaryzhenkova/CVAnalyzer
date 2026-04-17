using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> CreateAsync(DbRefreshToken token);

        Task<bool> RemoveAsync(List<DbRefreshToken> tokens, CancellationToken ct);

        Task<DbRefreshToken?> GetAsync(string requestToken);

        Task<List<DbRefreshToken>> GetExpiredAsync(CancellationToken ct);
    }
}