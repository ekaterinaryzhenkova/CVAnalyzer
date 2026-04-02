using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Models.Token;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CVAnalyzer.Business.Auth
{
    public class RegisterCommand(
        IUserRepository userRepository,
        IDbUserMapper mapper,
        IPasswordHasher<DbUser> hasher,
        IJwtService jwtService)
        : IRegisterCommand
    {
        public async Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(RegisterRequest request)
        {
            if (await userRepository.IsLoginAlreadyExistsAsync(request.Login))
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Login already exists.",
                    ResultStatus.BadRequest);
            }

            DbUser user = mapper.Map(request);
            user.UsersCredentials.PasswordHash = hasher.HashPassword(user, request.Password);

            LoginResultResponse result = new LoginResultResponse
            {
                UserId = user.Id,
                AccessToken = jwtService.GenerateToken(user, TokenType.Access, out double accessExpiresInMinutes),
                AccessTokenExpiresIn = accessExpiresInMinutes,
                RefreshToken = jwtService.GenerateToken(user, TokenType.Refresh, out double refreshExpiresInMinutes),
                RefreshTokenExpiresIn = refreshExpiresInMinutes
            };
            
            user.RefreshTokens.Add(new DbRefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Value = result.RefreshToken,
                ExpirationDate = DateTime.Now.AddMinutes(result.RefreshTokenExpiresIn)
            });
            
            await userRepository.CreateAsync(user);

            return new OperationResultResponse<LoginResultResponse>(result);
        }
    }
}