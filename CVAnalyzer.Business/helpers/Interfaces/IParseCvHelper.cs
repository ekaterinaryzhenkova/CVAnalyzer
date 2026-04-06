using Microsoft.AspNetCore.Http;

namespace CVAnalyzer.Business.helpers.Interfaces
{
    public interface IParseCvHelper
    {
        Task<string> ParseCvByPdf(IFormFile file);

        Task<string> ParseCvByDocx(IFormFile file);
    }
}