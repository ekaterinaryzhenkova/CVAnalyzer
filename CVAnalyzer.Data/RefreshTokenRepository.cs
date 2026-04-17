using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.Repositories
{
    public class RefreshTokenRepository(CVAnalyzerContext context) : IRefreshTokenRepository
    {
        public async Task<bool> CreateAsync(DbRefreshToken token)
        {
            await context.RefreshTokens
                .Where(t => t.UserId == token.UserId && !t.IsRevoked)
                .ExecuteUpdateAsync(t =>
                    t.SetProperty((x => x.IsRevoked), true));
            
            await context.RefreshTokens.AddAsync(token);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(List<DbRefreshToken> tokens, CancellationToken ct)
        {
            if (tokens is null || tokens.Count == 0)
            {
                return false;
            }

            context.RefreshTokens.RemoveRange(tokens);

            await context.SaveChangesAsync(ct);

            return true;
        }

        public Task<DbRefreshToken?> GetAsync(string requestToken)
        {
            return context.RefreshTokens
                .Include(rt => rt.User)
                .ThenInclude(u => u.UsersCredentials)
                .Where(rt => rt.Value == requestToken)
                .FirstOrDefaultAsync();
        }

        public Task<List<DbRefreshToken>> GetExpiredAsync(CancellationToken ct)
        {
            return context.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.ExpirationDate < DateTime.UtcNow)
                .ToListAsync(ct);
        }
    }
}