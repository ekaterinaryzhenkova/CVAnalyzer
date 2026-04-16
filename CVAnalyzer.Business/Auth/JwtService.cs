using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CVAnalyzer.Business.Auth
{
    public class JwtService(IOptions<JwtOptions> options) : IJwtService
    {
        public string GenerateToken(DbUser user, TokenType tokenType, out double tokenExpiresInMinutes)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UsersCredentials.Login),
                new Claim("TokenType", tokenType.ToString())
            };
            
            tokenExpiresInMinutes = tokenType == TokenType.Access
                ? options.Value.AccessTokenLifetimeInMinutes
                : options.Value.RefreshTokenLifetimeInMinutes;

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.Value.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpiresInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}