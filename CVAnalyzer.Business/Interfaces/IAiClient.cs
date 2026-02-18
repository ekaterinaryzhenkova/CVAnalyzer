using System.Net;

namespace CVAnalyzer.Business.Interfaces
{
    public interface IAiClient
    {
        Task<(HttpStatusCode StatusCode, string Content)> SendMessageAsync(string prompt);
    }
}