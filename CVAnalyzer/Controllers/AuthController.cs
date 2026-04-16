using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.Helpers;
using CVAnalyzer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Controllers
{
    /// <summary>
    /// Registration and authorization.
    /// </summary>
    [ApiController]
    [Route("auth")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// User registration.
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(
            [FromServices] IRegisterCommand command,
            [FromBody][Required] RegisterRequest request)
        {
            var result = await command.ExecuteAsync(request);

            return result.ToActionResult();
        }

        /// <summary>
        /// Refresh user token.
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshTokenAsync(
            [FromServices] IRefreshTokenCommand command,
            [FromBody] RefreshRequest refreshToken)
        {
            var result = await command.ExecuteAsync(refreshToken);
            
            return result.ToActionResult();
        }
        
        /// <summary>
        /// User authentication.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> LoginUserAsync(
            [FromServices] ILoginCommand command,
            [FromBody] LoginRequest request)
        {
            var result = await command.ExecuteAsync(request);
            
            return result.ToActionResult();
        }
    }
}