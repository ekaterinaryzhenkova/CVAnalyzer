using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface ICvRepository
    {
        Task<DbCV> CreateAsync(DbCV cv);

        Task<DbCV?> GetAsync(Guid id);
    }
}