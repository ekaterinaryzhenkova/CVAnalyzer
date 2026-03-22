using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.Requests;
using Microsoft.AspNetCore.Identity;

namespace CVAnalyzer.Mappers
{
    public class DbUserMapper : IDbUserMapper
    {
        public DbUser Map(RegisterRequest request)
        {
            Guid userId = Guid.NewGuid();
            
            DbUser user = new DbUser()
            {
                Id = userId,
                FirstName = request.Name,
                LastName = "",
                UsersCredentials = new DbUserCredentials()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Login = request.Login,
                }
            };

            return user;
        }
    }
}