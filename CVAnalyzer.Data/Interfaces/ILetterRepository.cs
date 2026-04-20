using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface ILetterRepository
    {
        Task<Guid> CreateAsync(DbLetter letter);
    }
}