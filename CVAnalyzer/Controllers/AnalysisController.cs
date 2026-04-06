using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Controllers
{
    /// <summary>
    /// Vacancies.
    /// </summary>
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("analysis")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AnalysisController : ControllerBase
    {
        /// <summary>
        /// Post analysis.
        /// </summary>
        [HttpPost]
        public async Task<OperationResultResponse<Guid>> CreateAsync(
            [FromServices] IStartAnalysisCommand createAnalysisCommand,
            [FromBody][Required] VacancyRequest vacancyRequest)
        {
            return await createAnalysisCommand.ExecuteAsync(vacancyRequest);
        }
        
        /// <summary>
        /// Get analysis.
        /// </summary>
        [HttpGet]
        public async Task<OperationResultResponse<AnalysisResponse>> GetAsync(
            [FromServices] IGetAnalysisCommand getAnalysisCommand,
            [FromQuery][Required] Guid analysisId)
        {
            return await getAnalysisCommand.ExecuteAsync(analysisId);
        }
    }
}