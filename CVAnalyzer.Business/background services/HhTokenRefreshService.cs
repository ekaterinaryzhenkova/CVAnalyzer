using CVAnalyzer.Models.AIClient;
using CVAnalyzer.Models.HhClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CVAnalyzer.Business.background_services
{
    public class HhTokenRefreshService(
        IHhTokenSettings tokenSettings,
        IHttpClientFactory httpClientFactory,
        IOptions<HhApiOptions> options,
        ILogger<HhTokenRefreshService> logger
        ) : IHostedService
    {
        private readonly HhApiOptions _apiOptions = options.Value;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("HhTokenRefreshService starting...");
            
            await RefreshTokenAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("HhTokenRefreshService stopping...");

            return Task.CompletedTask;
        }
        
        private async Task RefreshTokenAsync(CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient("HhApi");
                var request = new HttpRequestMessage(HttpMethod.Post, _apiOptions.TokenUrl);
                
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("client_id", _apiOptions.ClientId));
                collection.Add(new("client_secret", _apiOptions.ClientSecret));
                collection.Add(new("grant_type", "client_credentials"));
                
                var content = new FormUrlEncodedContent(collection);
                
                request.Content = content;
                
                var response = await client.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                using var doc = JsonDocument.Parse(json);

                var token = doc.RootElement
                    .GetProperty("access_token")
                    .GetString();
                
                if (!string.IsNullOrWhiteSpace(token))
                {
                    tokenSettings.AccessToken = token;
                    logger.LogInformation("Token refreshed successfully.");
                }
                else
                {
                    logger.LogWarning("Token refresh response did not contain a valid token.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting Hh token.");
            }
        } 
    }
}