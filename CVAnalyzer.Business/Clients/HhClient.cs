using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Models.HhClient;
using System.Net;
using System.Net.Http.Headers;

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

            return (response.StatusCode, content);
        }
    }
}