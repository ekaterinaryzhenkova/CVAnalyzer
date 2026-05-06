using CVAnalyzer.Business.Analysis.Interfaces;
using CVAnalyzer.Business.Letter;
using CVAnalyzer.Business.Letter.Interfaces;
using CVAnalyzer.Helpers;
using CVAnalyzer.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Controllers
{
    /// <summary>
    /// Letters.
    /// </summary>
    [ApiController]
    [Route("letters")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class LetterController : ControllerBase
    {
        /// <summary>
        /// Create letter
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync(
            [FromServices] ICreateLetterCommand createLetterCommand,
            [FromQuery][Required] Guid analysisId)
        {
            var result = await createLetterCommand.ExecuteAsync(analysisId);
            
            return result.ToActionResult();
        }
        
        /// <summary>
        /// Get letter.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(LetterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(
            [FromServices] IGetLetterCommand getLetterCommand,
            [FromQuery][Required] Guid letterId)
        {
            var result = await getLetterCommand.ExecuteAsync(letterId);
            
            return result.ToActionResult();
        }
    }
}