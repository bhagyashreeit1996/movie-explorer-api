using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Interfaces;
using System.Security.Claims;

namespace MovieExplorer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            await _authService.RegisterAsync(request);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
        LoginRequest request)
        {
            var response =
                await _authService
                    .LoginAsync(request);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var user = await _authService
                .GetCurrentUserAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}