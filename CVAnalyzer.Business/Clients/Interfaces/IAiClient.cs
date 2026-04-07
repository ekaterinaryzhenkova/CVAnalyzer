using System.Net;

namespace CVAnalyzer.Business.Clients.Interfaces
{
    public interface IAiClient
    {
        Task<(HttpStatusCode StatusCode, string Content)> SendMessageAsync(string prompt);

        Task<string> CreateAnalysisAsync(string prompt);
    }
}