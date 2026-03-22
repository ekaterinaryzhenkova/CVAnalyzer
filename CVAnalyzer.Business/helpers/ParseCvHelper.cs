using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace CVAnalyzer.Business.helpers
{
    public class ParseCvHelper : IParseCvHelper
    {
        public async Task<string> ParseCvByPdf(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
        
            var text = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(memoryStream))
            {
                foreach (Page page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }
            
            return text.ToString();
        }
        
        public async Task<string> ParseCvByDocx(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var wordDoc = WordprocessingDocument.Open(memoryStream, false);
            var body = wordDoc.MainDocumentPart?.Document.Body;

            if (body is null)
            {
                return string.Empty;
            }

            return body.InnerText;
        }
    }
}