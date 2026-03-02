using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.Auth.Interfaces
{
    public interface IRegisterCommand
    {
        Task<OperationResultResponse<LoginResultResponse>> ExecuteAsync(RegisterRequest request);
    }
}