using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Models.HhClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CVAnalyzer.Business.background_services
{
    public class HhTokenRefreshService(
        IHhTokenSettings tokenSettings,
        IHhClient client,
        IOptions<HhApiOptions> options,
        ILogger<HhTokenRefreshService> logger)
        : BackgroundService
    {
        private readonly HhApiOptions _apiOptions = options.Value;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("HhTokenRefreshService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "token");

                    request.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("client_id", _apiOptions.ClientId),
                        new KeyValuePair<string, string>("client_secret", _apiOptions.ClientSecret),
                        new KeyValuePair<string, string>("grant_type", "client_credentials")
                    });

                    var token = await client.GetTokenAsync(request, stoppingToken);

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokenSettings.AccessToken = token;

                        logger.LogInformation("Token successfully received. Stopping retries.");

                        return;
                    }

                    logger.LogWarning("Token is empty");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while getting token");
                }
                
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}