using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Token;

namespace CVAnalyzer.Business.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(DbUser user, TokenType tokenType, out double tokenExpiresInMinutes);
    }
}