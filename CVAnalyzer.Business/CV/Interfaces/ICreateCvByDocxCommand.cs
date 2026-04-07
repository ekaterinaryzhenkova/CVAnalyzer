using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace CVAnalyzer.Business.CV.Interfaces
{
    public interface ICreateCvByDocxCommand
    {
        Task<OperationResultResponse<Guid>> ExecuteAsync(IFormFile uploadedFile);
    }
}