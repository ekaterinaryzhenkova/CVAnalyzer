using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Models.Token;
using CVAnalyzer.Repositories.Interfaces;

namespace CVAnalyzer.Business.Auth
{
    public class RefreshTokenCommand(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService)
        : IRefreshTokenCommand
    {
        public async Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(RefreshRequest request)
        {
            var token = await refreshTokenRepository.GetAsync(request.RefreshToken);

            if (token is null)
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Invalid refresh token.",
                    ResultStatus.NotFound);
            }
            
            if (token.IsRevoked)
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Refresh token is revoked.",
                    ResultStatus.BadRequest);
            }

            if (token.ExpirationDate < DateTime.UtcNow)
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Refresh token expired.",
                    ResultStatus.Unauthorized);
            }
            
            LoginResultResponse result = new LoginResultResponse
            {
                UserId = token.UserId,
                AccessToken = jwtService.GenerateToken(token.User, TokenType.Access, out double accessExpiresInMinutes),
                AccessTokenExpiresIn = accessExpiresInMinutes,
                RefreshToken = jwtService.GenerateToken(token.User, TokenType.Refresh, out double refreshExpiresInMinutes),
                RefreshTokenExpiresIn = refreshExpiresInMinutes
            };
            
            var refreshToken = new DbRefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = token.UserId,
                Value = result.RefreshToken,
                ExpirationDate = DateTime.Now.AddMinutes(result.RefreshTokenExpiresIn)
            };
            
            await refreshTokenRepository.CreateAsync(refreshToken);

            return new OperationResultResponse<LoginResultResponse>(result);
        }
    }
}