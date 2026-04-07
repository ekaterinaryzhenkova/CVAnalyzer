using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CVAnalyzer.Repositories.Services
{
    public class PromptService(
        IPromptRepository repository,
        IMemoryCache cache)
        : IPromptService
    {
        private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(25);
        
        public async Task<string> GetAsync(string name)
        {
            if (cache.TryGetValue(name, out string? cachedPrompt) && cachedPrompt != null)
            {
                return cachedPrompt;
            }
            
            var prompt = await repository.GetAsync(name);

            cache.Set(name, prompt, CacheLifetime);

            return prompt;
        }

        public Task ClearAsync(string name)
        {
            cache.Remove(name);

            return Task.CompletedTask;
        }
    }
}