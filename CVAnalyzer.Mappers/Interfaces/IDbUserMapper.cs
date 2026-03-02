using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Requests;

namespace CVAnalyzer.Mappers.Interfaces
{
    public interface IDbUserMapper
    {
        DbUser Map(RegisterRequest request);
    }
}