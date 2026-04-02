using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using CVAnalyzer.Models.Responses;
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
        public async Task<OperationResultResponse<LoginResultResponse>> RegisterAsync(
            [FromServices] IRegisterCommand command,
            [FromBody][Required] RegisterRequest request)
        {
            return await command.ExecuteAsync(request);
        }

        /// <summary>
        /// Refresh user token.
        /// </summary>
        [HttpPost("refresh")]
        public async Task<OperationResultResponse<LoginResultResponse>> RefreshTokenAsync(
            [FromServices] IRefreshTokenCommand command,
            [FromBody] RefreshRequest refreshToken)
        {
            return await command.ExecuteAsync(refreshToken);
        }
        
        /// <summary>
        /// User authentication.
        /// </summary>
        [HttpPost("login")]
        public async Task<OperationResultResponse<LoginResultResponse>> LoginUserAsync(
            [FromServices] ILoginCommand command,
            [FromBody] LoginRequest request)
        {
            return await command.ExecuteAsync(request);
        }
    }
}