using CarServiceApi.DTOs;
using CarServiceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            try
            {
                await _authService.RegisterAsync(request);
                return Ok("User successfully registered!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            try
            {
                var token = await _authService.LoginAsync(request);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}