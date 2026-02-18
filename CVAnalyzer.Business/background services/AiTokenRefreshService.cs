using CVAnalyzer.Models;
using CVAnalyzer.Models.AIClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CVAnalyzer.Business.background_services
{
    public class AiTokenRefreshService(
        IAiTokenSettings aiToken,
        IHttpClientFactory httpClientFactory,
        IOptions<AiApiOptions> options,
        ILogger<AiTokenRefreshService> logger)
        : IHostedService, IDisposable
    {
        private readonly AiApiOptions _apiOptions = options.Value;
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("TokenRefreshService starting...");
            
            _timer = new Timer(async _ => await RefreshTokenAsync(),
                null,
                TimeSpan.Zero,   
                TimeSpan.FromMinutes(25));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("TokenRefreshService stopping...");

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
        
        private async Task RefreshTokenAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient("GigaChatJes");

                var request = new HttpRequestMessage(HttpMethod.Post, _apiOptions.TokenUrl);
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiOptions.ClientAuth);
                request.Headers.Add("RqUID", Guid.NewGuid().ToString());

                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "scope", _apiOptions.Scope }
                });

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var token = doc.RootElement
                    .GetProperty("access_token")
                    .GetString();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    aiToken.AccessToken = token;
                    logger.LogInformation("Token refreshed successfully.");
                }
                else
                {
                    logger.LogWarning("Token refresh response did not contain a valid token.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while refreshing GigaChat token.");
            }
        }
    }
}