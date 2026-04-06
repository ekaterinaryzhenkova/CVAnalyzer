using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Business.background_services.Interfaces;
using CVAnalyzer.Business.helpers.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.Analysis
{
    public class StartAnalysisCommand(
        IAnalysisRepository analysisRepository,
        IServiceScopeFactory scopeFactory,
        IBackgroundTaskQueue taskQueue,
        ILogger<StartAnalysisCommand> logger)
        : IStartAnalysisCommand
    {
        public async Task<OperationResultResponse<Guid>> ExecuteAsync(VacancyRequest request)
        {
            var analysis = new DbAnalysis
            {
                Id = Guid.NewGuid(),
                CvId = request.CvId,
                VacancyLink = request.Link,
                Status = AnalysisStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await analysisRepository.CreateAsync(analysis);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while saving analysis");
                return new OperationResultResponse<Guid>(
                    "Error while saving analysis",
                    ResultStatus.InternalServerError);
            }
            
            taskQueue.EnqueueTask(async () =>
            {
                using var scope = scopeFactory.CreateScope();

                var helper = scope.ServiceProvider.GetRequiredService<ICreateAnalysisHelper>();

                await helper.ExecuteAsync();
            });
            
            return new OperationResultResponse<Guid>(analysis.Id);
        }
    }
}