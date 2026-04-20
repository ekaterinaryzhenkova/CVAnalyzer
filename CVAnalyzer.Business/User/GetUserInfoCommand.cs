using CVAnalyzer.Business.User.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;

namespace CVAnalyzer.Business.User
{
    public class GetUserInfoCommand(IUserRepository repository) : IGetUserInfoCommand
    {
        public async Task<OperationResultResponse<UserInfo>> ExecuteAsync(Guid userId)
        {
            var userInfo = await repository.GetUserInfoAsync(userId);

            if (userInfo is null)
            {
                return new OperationResultResponse<UserInfo>(
                    "No user info was found",
                    ResultStatus.NotFound);
            }

            return new OperationResultResponse<UserInfo>(userInfo);
        }
    }
}