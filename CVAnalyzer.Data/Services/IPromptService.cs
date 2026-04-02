namespace CVAnalyzer.Repositories.Services
{
    public interface IPromptService
    {
        Task<string> GetAsync(string name);

        Task ClearAsync(string name);
    }
}