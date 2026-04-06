using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Business.CV.Interfaces
{
    public interface ICreateCvByManualInputCommand
    {
        Task<OperationResultResponse<Guid>> ExecuteAsync(ManualCvRequest manualCvRequest);
    }
}