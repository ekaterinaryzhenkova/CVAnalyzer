using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Business.Letter.Interfaces;
using CVAnalyzer.Models.RabbitMq;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace CVAnalyzer.Business.broker
{
    public class AiConsumer(
        ICreateLetterService createLetterService,
        ICreateAnalysisService createAnalysisService)
        : IConsumer<CreateAnalysisMessage>, IConsumer<CreateLetterMessage>
    {
        public async Task Consume(ConsumeContext<CreateAnalysisMessage> context)
        { 
            await createAnalysisService.ExecuteAsync(context.Message.AnalysisId);
        }
        
        public async Task Consume(ConsumeContext<CreateLetterMessage> context)
        {
            await createLetterService.ExecuteAsync(context.Message.LetterId);
        }
    }
}