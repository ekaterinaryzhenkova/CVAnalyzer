using System.Net;

namespace CVAnalyzer.Business.Interfaces
{
    public interface IHhClient
    {
        Task<(HttpStatusCode StatusCode, string Content)> ParseVacancyAsync(string vacancyId);
    }
}