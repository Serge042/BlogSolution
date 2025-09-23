using BlogApp.Business.Services;
using BlogApp.Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("auth/login")]

        public async Task<IActionResult> Login()
        {
            return View();
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                var response = await _authService.AuthenticateAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при входе: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterUserAsync(request);
                if (result)
                    return Ok("Пользователь успешно зарегистрирован");
                else
                    return BadRequest("Ошибка при регистрации пользователя");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при регистрации: {ex.Message}");
            }
        }
    }
}