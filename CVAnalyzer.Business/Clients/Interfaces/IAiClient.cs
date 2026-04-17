using System.Net;

namespace CVAnalyzer.Business.Clients.Interfaces
{
    public interface IAiClient
    {
        Task<string> CreateLetterAsync(string prompt);

        Task<string> CreateAnalysisAsync(string prompt);
    }
}