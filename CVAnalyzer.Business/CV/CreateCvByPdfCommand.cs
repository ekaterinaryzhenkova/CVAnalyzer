using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.helpers.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CVAnalyzer.Business.CV;

public class CreateCvByPdfCommand(
    ICvRepository cvRepository,
    IParseCvHelper parseHelper,
    IMemoryCache cache,
    IHttpContextAccessor httpContext,
    ILogger<CreateCvByPdfCommand> logger) 
    : ICreateCvByPdfCommand
{
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(25);
    
    public async Task<OperationResultResponse<Guid>> ExecuteAsync(IFormFile uploadedFile)
    {
        string parsedText =  await parseHelper.ParseCvByPdf(uploadedFile);
            
        if (parsedText.Length == 0)
        {
            return new OperationResultResponse<Guid>(
                "The uploaded file is empty.",
                ResultStatus.BadRequest);
        }
            
        string? valueFromContext = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid? userId = string.IsNullOrEmpty(valueFromContext) || !Guid.TryParse(valueFromContext, out Guid id)
            ? null
            : id;
        
        DbCV dbCv = new DbCV
        {
            Id = Guid.NewGuid(),
            UserId = userId,
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
                ResultStatus.InternalServerError);
        }

        return new OperationResultResponse<Guid>(dbCv.Id);
    }
}