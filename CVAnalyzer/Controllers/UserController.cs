using CVAnalyzer.Business.User.Interfaces;
using CVAnalyzer.Helpers;
using CVAnalyzer.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CVAnalyzer.Controllers
{
    /// <summary>
    /// Users.
    /// </summary>
    [ApiController]
    [Route("users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UserController: ControllerBase
    {
        /// <summary>
        /// Get user's analysis.
        /// </summary>
        [HttpGet("analyses")]
        [Authorize]
        [ProducesResponseType(typeof(List<ComplexAnalysisResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(
            [FromServices] IGetUserAnalysisCommand getUserAnalysisCommand,
            [FromQuery][Required] Guid userId)
        {
            var result = await getUserAnalysisCommand.ExecuteAsync(userId);
            
            return result.ToActionResult();
        }
        
        /// <summary>
        /// Get user's info.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAsync(
            [FromServices] IGetUserInfoCommand getUserInfoCommand,
            [FromQuery][Required] Guid userId)
        {
            var result = await getUserInfoCommand.ExecuteAsync(userId);
            
            return result.ToActionResult();
        }
    }
}