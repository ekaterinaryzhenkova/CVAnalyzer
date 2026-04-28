using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Models.HhClient;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CVAnalyzer.Business.Clients
{
    public class HhClient(
        HttpClient httpClient,
        IHhTokenSettings hhTokenSettings)
        : IHhClient
    {
        public async Task<(HttpStatusCode StatusCode, string Content)> ParseVacancyAsync(string vacancyId)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "vacancies/" + vacancyId);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", hhTokenSettings.AccessToken);

            using var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            
            using var doc = JsonDocument.Parse(content);

            var description = doc.RootElement
                .GetProperty("description")
                .GetString();
            
            var skills = string.Join(", ",
                doc.RootElement
                    .GetProperty("key_skills")
                    .EnumerateArray()
                    .Select(x => x.TryGetProperty("name", out var name) ? name.GetString() : null)
                    .Where(x => !string.IsNullOrEmpty(x))
            );

            if (description?.Length == 0 && skills.Length == 0)
            {
                return (response.StatusCode, content);
            }
            
            var text = "Описание: " + description + "/n Требуемые навыки: " + skills;

            return (response.StatusCode, text);
        }
        
        public async Task<string> ParseVacancyTextAsync(string vacancyId)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "vacancies/" + vacancyId);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", hhTokenSettings.AccessToken);

            using var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            
            using var doc = JsonDocument.Parse(content);

            var description = doc.RootElement
                .GetProperty("description")
                .GetString();
            
            var skills = string.Join(", ",
                doc.RootElement
                    .GetProperty("key_skills")
                    .EnumerateArray()
                    .Select(x => x.TryGetProperty("name", out var name) ? name.GetString() : null)
                    .Where(x => !string.IsNullOrEmpty(x))
            );

            if (description?.Length == 0 && skills.Length == 0)
            {
                return content;
            }
            
            var text = "Описание: " + description + "/n Требуемые навыки: " + skills;

            return text;
        }
        
        public async Task<string?> GetTokenAsync(HttpRequestMessage request, CancellationToken ct)
        {
            using var response = await httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            var token = doc.RootElement
                .GetProperty("access_token")
                .GetString();

            return token;
        }
    }
}