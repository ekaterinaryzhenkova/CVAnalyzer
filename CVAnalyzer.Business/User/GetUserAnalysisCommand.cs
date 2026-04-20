using CVAnalyzer.Business.User.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.User
{
    public class GetUserAnalysisCommand(
        ICvRepository cvRepository,
        IMemoryCache cache,
        ILogger<GetUserAnalysisCommand> logger)
        : IGetUserAnalysisCommand
    {
        public async Task<OperationResultResponse<List<ComplexAnalysisResponse>>> ExecuteAsync(Guid userId)
        {
            var analyses = await cvRepository.GetComplexAnalysisAsync(userId);

            if (!analyses.Any())
            {
                return new OperationResultResponse<List<ComplexAnalysisResponse>>(
                    "No analysis was found",
                    ResultStatus.NotFound);
            }
            
            return new OperationResultResponse<List<ComplexAnalysisResponse>>(analyses);
        }
    }
}