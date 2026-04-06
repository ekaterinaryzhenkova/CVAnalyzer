using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Business.background_services.Interfaces;
using CVAnalyzer.Business.helpers.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace CVAnalyzer.Business.Analysis
{
    public class StartAnalysisCommand(
        IAnalysisRepository analysisRepository,
        IServiceScopeFactory scopeFactory,
        IBackgroundTaskQueue taskQueue)
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
            
            await analysisRepository.CreateAsync(analysis);
            
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