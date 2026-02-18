using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IPromptRepository
    {
        Task<string?> GetAsync(string name);
    }
}