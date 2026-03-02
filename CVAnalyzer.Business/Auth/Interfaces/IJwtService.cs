using CVAnalyzer.DbLayer.Models;

namespace CVAnalyzer.Business.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(DbUser user);
    }
}