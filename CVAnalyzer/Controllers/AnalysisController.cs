using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Helpers;
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
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync(
            [FromServices] IStartAnalysisCommand createAnalysisCommand,
            [FromBody][Required] VacancyRequest vacancyRequest)
        {
            var result = await createAnalysisCommand.ExecuteAsync(vacancyRequest);

            return result.ToActionResult();
        }
        
        /// <summary>
        /// Get analysis.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(AnalysisResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(
            [FromServices] IGetAnalysisCommand getAnalysisCommand,
            [FromQuery][Required] Guid analysisId)
        {
            var result = await getAnalysisCommand.ExecuteAsync(analysisId);
            
            return result.ToActionResult();
        }
    }
}