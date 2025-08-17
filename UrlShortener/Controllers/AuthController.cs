using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs;
using UrlShortener.Services.Auth;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            var userId = await _authService.Register(userDto);
            return Ok(new { UserId =  userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRegisterDto userDto)
        {
            var token = await _authService.Login(userDto); 
            return Ok(new { Token = token }); 
        }
    }
}
