using System.Net;

namespace CVAnalyzer.Business.Clients.Interfaces
{
    public interface IHhClient
    {
        Task<(HttpStatusCode StatusCode, string Content)> ParseVacancyAsync(string vacancyId);
    }
}