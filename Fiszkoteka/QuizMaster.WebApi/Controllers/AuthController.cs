using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody] LogoutRequest request,
            CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(request, cancellationToken);
            return Ok();
        }
    }
}
