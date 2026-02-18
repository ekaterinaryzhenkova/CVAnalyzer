using CVAnalyzer.Business;
using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// Create CV by pdf.
        /// </summary>
        [HttpPost("pdf")]
        public async Task<OperationResultResponse<AnalysisResponse>> CreateByPdfAsync(
            [FromServices] ICreateCVbyPDFCommand command,
            [FromForm] IFormFile uploadedFile)
        {
            return await command.ExecuteAsync(uploadedFile);
        }

        /// <summary>
        /// Create CV by docx.
        /// </summary>
        [HttpPost("docx")]
        public async Task<OperationResultResponse<AnalysisResponse>> CreateByDocxAsync(
            [FromServices] ICreateCVbyDocxCommand command,
            [FromForm] IFormFile uploadedFile)
        {
            return await command.ExecuteAsync(uploadedFile);
        }

        /// <summary>
        /// Create CV by text.
        /// </summary>
        [HttpPost("manual")]
        public async Task<OperationResultResponse<AnalysisResponse>> CreateByManualAsync(
            [FromServices] ICreateCVbyManualInputCommand command,
            [FromBody][Required] ManualCvRequest manualCvRequest)
        {
            return await command.ExecuteAsync(manualCvRequest);
        }
    }
}