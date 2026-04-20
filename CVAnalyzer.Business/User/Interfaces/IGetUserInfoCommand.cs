using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.User.Interfaces
{
    public interface IGetUserInfoCommand
    {
        Task<OperationResultResponse<UserInfo>> ExecuteAsync(Guid userId);
    }
}