using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace CVAnalyzer.Business.CV.Interfaces
{
    
    public interface ICreateCbByPdfCommand
    {
        Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(IFormFile uploadedFile);
    }
}