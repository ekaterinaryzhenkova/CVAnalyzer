using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Models.Token;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CVAnalyzer.Business.Auth
{
    public class LoginCommand(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtService jwtService,
        IPasswordHasher<DbUser> passwordHasher)
        : ILoginCommand
    {
        public async Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(LoginRequest request)
        {
            var user = await userRepository.GetByLoginAsync(request.Login);

            if (user is null)
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Invalid login or password",
                    ResultStatus.BadRequest);
            }
            
            var verificationResult = passwordHasher.VerifyHashedPassword(
                user,
                user.UsersCredentials.PasswordHash,
                request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Invalid login or password",
                    ResultStatus.BadRequest);
            }
            
            LoginResultResponse result = new LoginResultResponse
            {
                UserId = user.Id,
                AccessToken = jwtService.GenerateToken(user, TokenType.Access, out double accessExpiresInMinutes),
                AccessTokenExpiresIn = accessExpiresInMinutes,
                RefreshToken = jwtService.GenerateToken(user, TokenType.Refresh, out double refreshExpiresInMinutes),
                RefreshTokenExpiresIn = refreshExpiresInMinutes
            };

            var refreshToken = new DbRefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Value = result.RefreshToken,
                ExpirationDate = DateTime.Now.AddMinutes(result.RefreshTokenExpiresIn)
            };
            
            await refreshTokenRepository.CreateAsync(refreshToken);

            return new OperationResultResponse<LoginResultResponse>(result);
        }
    }
}