using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.Auth.Interfaces
{
    public interface ILoginCommand
    {
        Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(LoginRequest request);
    }
}