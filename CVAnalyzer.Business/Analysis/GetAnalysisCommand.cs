using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.Analysis
{
    public class GetAnalysisCommand(
        IAnalysisRepository repository,
        IAnalysisResponseMapper mapper,
        IMemoryCache cache,
        ILogger<GetAnalysisCommand> logger) 
        : IGetAnalysisCommand
    {
        public async Task<OperationResultResponse<AnalysisResponse>> ExecuteAsync(Guid analysisId)
        {
            if (cache.TryGetValue(analysisId.ToString(), out AnalysisResponse? cachedAnalysis) &&
                cachedAnalysis is not null)
            {
                logger.LogInformation("Analysis was received from cache");
                return new OperationResultResponse<AnalysisResponse>(cachedAnalysis);
            }

            var dbAnalysis = await repository.GetAsync(analysisId);

            if (dbAnalysis is null)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "No analysis was found",
                    ResultStatus.NotFound);
            }

            if (dbAnalysis.Status == AnalysisStatus.Processing || dbAnalysis.Status == AnalysisStatus.Created)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "Analysis in processing",
                    ResultStatus.InProgress);
            }

            if (dbAnalysis.Status == AnalysisStatus.Failed)
            {
                return new OperationResultResponse<AnalysisResponse>(
                    "Analysis is failed",
                    ResultStatus.Ok);
            }

            return new OperationResultResponse<AnalysisResponse>(mapper.Map(dbAnalysis));
        }
    }
}