using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CVAnalyzer.Business.background_services
{
    public class RefreshTokenRemovalService(
        ILogger<RefreshTokenRemovalService> logger,
        IServiceScopeFactory factory)
        : IHostedService, IDisposable
    {
         private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("RefreshTokenRemovalService starting...");
            
            _timer = new Timer(async _ => await RefreshTokenAsync(),
                null,
                TimeSpan.Zero,   
                TimeSpan.FromMinutes(15));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("RefreshTokenRemovalService stopping...");

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
        
        private async Task RefreshTokenAsync()
        {
            using var scope = factory.CreateScope();  

            var repository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();  
            
            var expiredTokens = await repository.GetExpiredAsync();
            
            if (expiredTokens.Count == 0)
            {
                logger.LogInformation("No expired refresh tokens found.");

                return;
            }

            await repository.RemoveAsync(expiredTokens);

            logger.LogInformation("Expired refresh tokens was removed.");
        }
    }
}