using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public AuthController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost("login")]
        public IActionResult Login(string userId)
        {
            _sessionService.Login(userId);
            return Ok("User logged in.");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _sessionService.Logout();
            return Ok("User logged out.");
        }

        [HttpGet("isLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(_sessionService.IsLoggedIn() ? "User is logged in." : "User is not logged in.");
        }
    }
}