namespace CVAnalyzer.Repositories.Interfaces
{
    public interface IPromptService
    {
        Task<string?> GetAsync(string name);
        Task ClearAsync(string name);
    }
}