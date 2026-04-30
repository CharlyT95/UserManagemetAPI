using Aduanas.Aci.Seguridad.Api.DTOs.Auth;
using Aduanas.Aci.Seguridad.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aduanas.Aci.Seguridad.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var token = _authService.Login(request.UsuarioLogin, request.Password);

            if (token == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(new
            {
                token = token
            });
        }
    }
}
