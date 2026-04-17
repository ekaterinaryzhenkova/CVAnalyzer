using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Models.AIClient;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace CVAnalyzer.Business.Clients
{
    public class AiClient(
        HttpClient httpClient,
        IAiTokenSettings aiTokenSettings)
        : IAiClient
    {
        public async Task<string> CreateLetterAsync(string prompt)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://gigachat.devices.sberbank.ru/api/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", aiTokenSettings.AccessToken);
            
            var payload = new
            {
                model = "GigaChat-2-Pro:2.0.28.2",
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
            
            if (!(response.StatusCode is HttpStatusCode.OK))
            {
                throw new HttpRequestException(
                    $"AI request failed. Status: {response.StatusCode}, Body: {content}",
                    null,
                    response.StatusCode);
            }

            try
            {
                var doc = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(content);
            
                if (doc.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0)
                {
                    var msg = choices[0].GetProperty("message").GetProperty("content");
                    return msg.GetString();
                }
                
                if (doc.TryGetProperty("error", out var error))
                {
                    throw new HttpRequestException($"AI API error: {error}");
                }

                throw new HttpRequestException($"Unexpected AI response: {content}");
            }
            catch(JsonException ex)
            {
                throw new JsonException($"Failed to parse AI response: {content}", ex);
            }
        }
        
        public async Task<string> CreateAnalysisAsync(string prompt)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://gigachat.devices.sberbank.ru/api/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", aiTokenSettings.AccessToken);
            
            var payload = new
            {
                model = "GigaChat-2-Pro:2.0.28.2",
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
            
            if (!(response.StatusCode is HttpStatusCode.OK))
            {
                throw new ExternalException(response.StatusCode.ToString());
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var doc = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(content);
            
            if (doc.TryGetProperty("choices", out var choices) &&
                choices.GetArrayLength() > 0)
            {
                var msg = choices[0].GetProperty("message").GetProperty("content");
                return msg.GetString();
            }

            if (doc.TryGetProperty("error", out var error))
            {
                throw new ExternalException($"AI API error: {error}");
            }
            
            throw new ExternalException($"Unexpected AI response: {content}");
        }
    }
}