using CVAnalyzer.Business.background_services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.background_services
{
    public class CreateAnalysisService(
        IBackgroundTaskQueue taskQueue,
        ILogger<CreateAnalysisService> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await taskQueue.DequeueTaskAsync(stoppingToken);
                
                try
                {
                    await task.Invoke();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Background task failed");
                }
            }
        }
    }
}