using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Controllers
{
    /// <summary>
    /// CVs.
    /// </summary>
    [ApiController]
    [Route("cv")]
    [Consumes("multipart/form-data", "application/json")]
    [Produces("application/json")]
    public class CvController: ControllerBase
    {
        /// <summary>
        /// Create CV by docx.
        /// </summary>
        [HttpPost("docx")]
        public async Task<OperationResultResponse<Guid>> CreateByDocxAsync(
            [FromServices] ICreateCvByDocxCommand command,
            IFormFile uploadedFile)
        { 
            return await command.ExecuteAsync(uploadedFile);
        }
        
        /// <summary>
        /// Create CV by pdf.
        /// </summary>
        [HttpPost("pdf")]
        public async Task<OperationResultResponse<Guid>> CreateByPdfAsync(
            [FromServices] ICreateCvByPdfCommand command,
            IFormFile uploadedFile)
        { 
            return await command.CreateCvAsync(uploadedFile);
        }
        
        /// <summary>
        /// Create CV by text.
        /// </summary>
        [HttpPost("manual")]
        public async Task<OperationResultResponse<Guid>> CreateByManualAsync(
            [FromServices] ICreateCvByManualInputCommand command,
            [FromBody][Required] ManualCvRequest manualCvRequest)
        {
            return await command.ExecuteAsync(manualCvRequest);
        }
    }
}