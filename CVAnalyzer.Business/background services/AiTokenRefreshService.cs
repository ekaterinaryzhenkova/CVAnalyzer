using CVAnalyzer.Models.AIClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CVAnalyzer.Business.background_services;

public class AiTokenRefreshService(
    IAiTokenSettings aiToken,
    IHttpClientFactory httpClientFactory,
    IOptions<AiApiOptions> options,
    ILogger<AiTokenRefreshService> logger)
    : BackgroundService
{
    private readonly AiApiOptions _apiOptions = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("AiTokenRefreshService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RefreshTokenAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(25), stoppingToken);
        }
    }

    private async Task RefreshTokenAsync(CancellationToken token)
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

            var response = await client.SendAsync(request, token);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(token);

            using var doc = JsonDocument.Parse(json);

            var tokenValue = doc.RootElement
                .GetProperty("access_token")
                .GetString();

            if (!string.IsNullOrWhiteSpace(tokenValue))
            {
                aiToken.AccessToken = tokenValue;
                logger.LogInformation("Token refreshed successfully.");
            }
        }
        catch (TaskCanceledException)
        {
            logger.LogInformation("Token refresh cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while refreshing GigaChat token");
        }
    }
}