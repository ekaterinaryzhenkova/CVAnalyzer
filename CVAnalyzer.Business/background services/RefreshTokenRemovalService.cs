using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.background_services
{
    public class RefreshTokenRemovalService(
        ILogger<RefreshTokenRemovalService> logger,
        IServiceScopeFactory factory)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            logger.LogInformation("RefreshTokenRemovalService started");

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using var scope = factory.CreateScope();
                    var repository = scope.ServiceProvider
                        .GetRequiredService<IRefreshTokenRepository>();

                    var expiredTokens = await repository.GetExpiredAsync(ct);

                    if (expiredTokens.Count == 0)
                    {
                        logger.LogInformation("No expired refresh tokens found.");
                    }
                    else
                    {
                        await repository.RemoveAsync(expiredTokens, ct);
                        logger.LogInformation("Expired refresh tokens were removed.");
                    }
                }
                catch (TaskCanceledException)
                {
                    logger.LogInformation("Token cleanup cancelled");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during token cleanup");
                }

                await Task.Delay(TimeSpan.FromMinutes(15), ct);
            }
        }
    }
}