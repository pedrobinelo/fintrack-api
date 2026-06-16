using FinTrack_API.DTOs;
using FinTrack_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack_API.Controllers
{
    [ApiController]
    [Route("api/auth")] // Rota base: api/auth
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController (AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")] // POST: api/auth/register
        public async Task<IActionResult> Register ([FromBody] RegisterDTO dto)
        {
            var user = await _authService.RegisterAsync(dto);
            if (user == null)
            {
                return BadRequest(new { message = "Este e-mail já está em uso." });
            }
            return StatusCode(201, new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost("login")] // POST: api/auth/login
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
            {
                return Unauthorized(new { message = "E-mail ou senha inválidos." });
            }
            return Ok(new { token });
        }
    }
}
