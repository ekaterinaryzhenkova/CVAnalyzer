using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.helpers.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.CV;

public class CreateCvByPdfCommand(
    ICvRepository cvRepository,
    IParseCvHelper parseHelper,
    IMemoryCache cache,
    ILogger<CreateCvByPdfCommand> logger) 
    : ICreateCvByPdfCommand
{
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(25);
    
    public async Task<OperationResultResponse<Guid>> CreateCvAsync(IFormFile uploadedFile)
    {
        string parsedText =  await parseHelper.ParseCvByPdf(uploadedFile);
            
        if (parsedText.Length == 0)
        {
            return new OperationResultResponse<Guid>(
                "The uploaded file is empty.",
                ResultStatus.BadRequest);
        }
            
        //TODO: пока без юзер айди, но взять из контекста, если подключить авторизацию
        DbCV dbCv = new DbCV
        {
            Id = Guid.NewGuid(),
            ParsedText = parsedText
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