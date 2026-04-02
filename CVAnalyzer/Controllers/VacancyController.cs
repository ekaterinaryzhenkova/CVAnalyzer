using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Controllers
{
    /// <summary>
    /// Vacancies.
    /// </summary>
    [ApiController]
    [Route("vacancy")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class VacancyController: ControllerBase
    {
        /// <summary>
        /// Get vacancy by link.
        /// </summary>
        [HttpGet]
        public async Task<OperationResultResponse<AnalysisResponse>> GetAsync(
            [FromServices] IParseVacancyAndCreateAnalysisCommand andCreateAnalysisCommand,
            [FromQuery][Required] VacancyRequest vacancyRequest)
        {
            return await andCreateAnalysisCommand.ExecuteAsync(vacancyRequest);
        }
    }
}