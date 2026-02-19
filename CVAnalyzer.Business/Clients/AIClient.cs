using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Models.AIClient;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace CVAnalyzer.Business.Clients
{
    public class AiClient(
        HttpClient httpClient,
        IAiTokenSettings aiTokenSettings)
        : IAiClient
    {
        public async Task<(HttpStatusCode StatusCode, string Content)> SendMessageAsync(string prompt)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://gigachat.devices.sberbank.ru/api/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", aiTokenSettings.AccessToken);
            
            var payload = new
            {
                model = "GigaChat-2",
                stream = false,
                update_interval = 0,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            request.Content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );
            
            using var response = await httpClient.SendAsync(request);
            
            var content = await response.Content.ReadAsStringAsync();
            
            var doc = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(content);
            
            if (doc.TryGetProperty("choices", out var choices) &&
                choices.GetArrayLength() > 0)
            {
                var msg = choices[0].GetProperty("message").GetProperty("content");
                return (response.StatusCode, msg.GetString());
            }

            return (response.StatusCode, content);
        }
    }
}