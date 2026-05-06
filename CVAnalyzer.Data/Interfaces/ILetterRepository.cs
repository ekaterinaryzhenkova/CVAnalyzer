using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface ILetterRepository
    {
        Task<Guid> CreateAsync(DbLetter letter);

        Task<DbLetter?> GetAsync(Guid id);

        Task<int> UpdateAsync(Guid letterId, LetterStatus status);

        Task<int> UpdateAsync(Guid letterId, string text, LetterStatus status);
    }
}