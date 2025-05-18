using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using APIGateWay.Model;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly SessionService _sessionService;

        public AuthController(AuthService authService, SessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var (success, message) = await _authService.Register(request.Name, request.Email, request.Password);
            if (!success)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var (success, message, email, name, balance,accountId) = await _authService.Login(request.Email, request.Password);
            if (!success)
                return Unauthorized(message);

            return Ok(new
            {
                Message = message,
                Email = email,
                Name = name,
                Balance = balance,
                Account = accountId
            });
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
        
        [HttpPost("insert-kyc")]
        public async Task<IActionResult> InsertKyc([FromBody] KycInsertRequest request)
        {
            // Fix DateOfBirth if invalid (e.g., MinValue or out of SQL range)
            if (request.Kyc.DateOfBirth < new DateTime(1753, 1, 1) || request.Kyc.DateOfBirth > new DateTime(9999, 12, 31))
            {
                request.Kyc.DateOfBirth = DateTime.Today;
            }

            // Also validate RecordDate (as before)
            if (request.RecordDate < new DateTime(1753, 1, 1) || request.RecordDate > new DateTime(9999, 12, 31))
            {
                request.RecordDate = DateTime.Now;
            }

            var (success, message) = await _authService.InsertKycAsync(request.Kyc, request.AccountId, request.RecordDate);
            if (!success)
                return BadRequest(message);

            return Ok(message);
        }


    }
}