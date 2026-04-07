using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Helpers;
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
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateByDocxAsync(
            [FromServices] ICreateCvByDocxCommand command,
            IFormFile uploadedFile)
        { 
            var result = await command.ExecuteAsync(uploadedFile);
            return result.ToActionResult();
        }
        
        /// <summary>
        /// Create CV by pdf.
        /// </summary>
        [HttpPost("pdf")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateByPdfAsync(
            [FromServices] ICreateCvByPdfCommand command,
            IFormFile uploadedFile)
        { 
            var result = await command.ExecuteAsync(uploadedFile);
            return result.ToActionResult();
        }
        
        /// <summary>
        /// Create CV by text.
        /// </summary>
        [HttpPost("manual")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateByManualAsync(
            [FromServices] ICreateCvByManualInputCommand command,
            [FromBody][Required] ManualCvRequest manualCvRequest)
        {
            var result = await command.ExecuteAsync(manualCvRequest);
            return result.ToActionResult();
        }
    }
}