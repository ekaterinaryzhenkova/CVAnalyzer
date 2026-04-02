using CVAnalyzer.Business;
using CVAnalyzer.Business.CV.Interfaces;
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
            [FromServices] ICreateCvByPdfCommand command,
            [FromForm] IFormFile uploadedFile)
        {
            return await command.ExecuteAsync(uploadedFile);
        }

        /// <summary>
        /// Create CV by docx.
        /// </summary>
        [HttpPost("docx")]
        public async Task<OperationResultResponse<AnalysisResponse>> CreateByDocxAsync(
            [FromServices] ICreateCvByDocxCommand command,
            [FromForm] IFormFile uploadedFile)
        {
            return await command.ExecuteAsync(uploadedFile);
        }

        /// <summary>
        /// Create CV by text.
        /// </summary>
        [HttpPost("manual")]
        public async Task<OperationResultResponse<AnalysisResponse>> CreateByManualAsync(
            [FromServices] ICreateCvByManualInputCommand command,
            [FromBody][Required] ManualCvRequest manualCvRequest)
        {
            return await command.ExecuteAsync(manualCvRequest);
        }
        
        [HttpPost("ia/docx")]
        public async Task<OperationResultResponse<Guid>> CreateByDocxIaAsync(
            [FromServices] ICreateCvByDocxCommand command,
            [FromForm] IFormFile uploadedFile)
        { 
            return await command.CreateCvAsync(uploadedFile);
        }
        
        [HttpPost("ia/pdf")]
        public async Task<OperationResultResponse<Guid>> CreateByPdfIaAsync(
            [FromServices] ICreateCvByPdfCommand command,
            [FromForm] IFormFile uploadedFile)
        { 
            return await command.CreateCvAsync(uploadedFile);
        }
        
        [HttpPost("ia/manual")]
        public async Task<OperationResultResponse<Guid>> CreateByManualIaAsync(
            [FromServices] ICreateCvByManualInputCommand command,
            [FromBody][Required] ManualCvRequest manualCvRequest)
        {
            return await command.CreateCvAsync(manualCvRequest);
        }
    }
}