using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
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
        //[Authorize]
        public async Task<OperationResultResponse<string>> GetAsync(
            [FromServices] IParseVacancyCommand command,
            [FromQuery][Required] VacancyRequest vacancyRequest)
        {
            return await command.ExecuteAsync(vacancyRequest);
        }
    }
}