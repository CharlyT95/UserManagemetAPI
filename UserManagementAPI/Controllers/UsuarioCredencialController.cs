using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial;
using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Helpers;

namespace Aduanas.Aci.Usuarios.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioCredencialController : ControllerBase
    {

        private readonly UsuarioCredencialService _usuarioCredencialService;

        public UsuarioCredencialController(UsuarioCredencialService usuarioCredencialService)
        {
            _usuarioCredencialService = usuarioCredencialService;
        }

        [HttpPost("Credenciales")]
        public async Task<IActionResult> Credenciales([FromBody] CreateUsuarioCredencialDTO credencial)
        {
            var data = await _usuarioCredencialService.CreateCredeciales(credencial);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPost("CambiarPassword")]
        public async Task<IActionResult> CambioPassword([FromBody] CambiarPasswordDTO password)
        {
            var data = await _usuarioCredencialService.ChangePassword(password);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var data = await _usuarioCredencialService.Login(login);
            return Ok(ResponseHelper.Success(data));
        }
    }
}
