using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;
using Microsoft.Extensions.Caching.Memory;

namespace CVAnalyzer.Repositories
{
    public class PromptService(
        IPromptRepository repository,
        IMemoryCache cache)
        : IPromptService
    {
        private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(25);
        
        public async Task<string?> GetAsync(string name)
        {
            if (cache.TryGetValue(name, out string? cachedPrompt))
            {
                return cachedPrompt;
            }
            
            var prompt = await repository.GetAsync(name);

            if (prompt is not null)
            {
                cache.Set(name, prompt, CacheLifetime);
            }

            return prompt;
        }

        public Task ClearAsync(string name)
        {
            cache.Remove(name);

            return Task.CompletedTask;
        }
    }
}