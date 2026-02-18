using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.Requests;

namespace CVAnalyzer.Repositories.Interfaces
{
    public interface ICVRepository
    {
        Task<DbCV> CreateAsync(ManualCvRequest cvRequest);

        Task<DbCV> CreateAsync(string cv);
    }
}