using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CVAnalyzer.Business.CV
{
    public class CreateCvByManualInputCommand(
        ICvRepository cvRepository,
        IMemoryCache cache,
        ILogger<CreateCvByManualInputCommand> logger,
        IHttpContextAccessor httpContext) 
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
            
            string? valueFromContext = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid? userId = string.IsNullOrEmpty(valueFromContext) || !Guid.TryParse(valueFromContext, out Guid id)
                ? null
                : id;

            DbCV dbCv = new DbCV
            {
                Id = Guid.NewGuid(),
                UserId = userId,
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
                    ResultStatus.InternalServerError);
            }

            return new OperationResultResponse<Guid>(dbCv.Id);
        }
    }
}