using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.Auth.Interfaces
{
    public interface IRefreshTokenCommand
    {
        Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(RefreshRequest request);
    }
}