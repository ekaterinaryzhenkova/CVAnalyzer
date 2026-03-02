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
        IUserRepository repository,
        IDbUserMapper mapper,
        IPasswordHasher<DbUser> hasher,
        IOptions<JwtOptions> options,
        IJwtService jwtService)
        : IRegisterCommand
    {
        public async Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(RegisterRequest request)
        {
            if (await repository.GetByLoginAsync(request.Login))
            {
                return new OperationResultResponse<LoginResultResponse>(
                    "Login already exists.",
                    ResultStatus.BadRequest);
            }

            DbUser user = mapper.Map(request);
            user.UsersCredentials.PasswordHash = hasher.HashPassword(user, request.Password);

            LoginResultResponse response = new LoginResultResponse
            {
                UserId = user.Id,
                AccessToken = jwtService.GenerateToken(user),
                AccessTokenExpiresIn = options.Value.ExpireMinutes
            };
            
            await repository.Create(user);

            return new OperationResultResponse<LoginResultResponse>(response);
        }
    }
}