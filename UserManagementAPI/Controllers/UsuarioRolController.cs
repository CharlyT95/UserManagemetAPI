using Aduanas.Aci.Usuarios.Api.DTOs;
using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol;
using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Helpers;

namespace Aduanas.Aci.Usuarios.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioRolController : ControllerBase
    {
        private readonly UsuarioRolService _usuarioRolService;

        public UsuarioRolController(UsuarioRolService usuarioRolService)
        {
            _usuarioRolService = usuarioRolService;
        }

        [HttpPost]
        public async Task<IActionResult> AsignarRolUsuario([FromBody] CreateUsuarioRolDTO usuarioRol)
        {
            var data = await _usuarioRolService.CreateUsuarioRol(usuarioRol);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesPorUsuario(int idUsuario)
        {
            var data = await _usuarioRolService.GetRolPorUsuario(idUsuario);
            return Ok(ResponseHelper.Success(data));

        }

        [HttpPost("estado")]
        public async Task<IActionResult> DeactivateRolToUsuario(int idUsuarioRol, [FromBody] CambiarEstadoDTO estado)
        {
            var data = await _usuarioRolService.CambiarEstadoUsuarioRol(idUsuarioRol,estado.activo);
            return Ok(ResponseHelper.Success(data));
        }
    }
}
