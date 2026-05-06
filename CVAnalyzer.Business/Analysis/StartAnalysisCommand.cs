using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Business.background_services.Interfaces;
using CVAnalyzer.Business.helpers.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.RabbitMq;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Repositories.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.Analysis
{
    public class StartAnalysisCommand(
        IAnalysisRepository analysisRepository,
        IPublishEndpoint publishEndpoint,
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
            
            await publishEndpoint.Publish(new CreateAnalysisMessage(analysis.Id));
            
            return new OperationResultResponse<Guid>(analysis.Id);
        }
    }
}