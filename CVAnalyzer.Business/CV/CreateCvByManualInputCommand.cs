using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.CV
{
    public class CreateCvByManualInputCommand(
        ICvRepository cvRepository,
        IMemoryCache cache,
        ILogger<CreateCvByManualInputCommand> logger) 
        : ICreateCvByManualInputCommand
    {
        private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(25);
        public async Task<OperationResultResponse<Guid>> ExecuteAsync(ManualCvRequest manualCvRequest)
        {
            string cv = "ФИО: " + manualCvRequest.FullName + "\n" +
                        "Позиция: " + manualCvRequest.Position + "\n" +
                        "Навыки: " + manualCvRequest.Skills + "\n" +
                        "Опыт: " + manualCvRequest.Experience + "\n" +
                        "Образование: " + manualCvRequest.Education + "\n" +
                        "О себе: " + manualCvRequest.AboutYourself;
            
            //TODO: пока без юзер айди, но взять из контекста, если подключить авторизацию
            DbCV dbCv = new DbCV
            {
                Id = Guid.NewGuid(),
                ParsedText = cv
            };

            try
            {
                await cvRepository.CreateAsync(dbCv);
                
                logger.LogInformation("Cv added to cache");
                cache.Set(dbCv.Id, dbCv, CacheLifetime);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new OperationResultResponse<Guid>(
                    "Error while saving cv to database",
                    ResultStatus.ExternalServerError);
            }

            return new OperationResultResponse<Guid>(dbCv.Id);
        }
    }
}