using System.Net;

namespace CVAnalyzer.Business.Clients.Interfaces
{
    public interface IHhClient
    {
        Task<(HttpStatusCode StatusCode, string Content)> ParseVacancyAsync(string vacancyId);

        Task<string> ParseVacancyTextAsync(string vacancyId);

        Task<string?> GetTokenAsync(HttpRequestMessage request, CancellationToken ct);
    }
}