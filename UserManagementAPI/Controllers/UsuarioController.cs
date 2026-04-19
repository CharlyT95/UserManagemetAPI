
using Aduanas.Aci.Usuarios.Api.DTOs;
using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.DTOs.Usuario;
using UserManagementAPI.Helpers;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _userService;

        public UsuarioController(UsuarioService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var data = await _userService.GetUsuariosAsync();

            return Ok(ResponseHelper.Success(
                data,
                data.Count == 0 ? "No hay datos que mostrar" : "Datos obtenidos correctamente"
            ));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getUserById(int id)
        {
            var data = await _userService.GetUsuarioByIdAsync(id);
            return Ok(ResponseHelper.Success(
                data,
                data == null ? "No existe el usuario que mostrar" : "Usuario obtenido correctamente"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioDTO usuario)
        {
            var data = await _userService.CreateUserAsync(usuario);
            return Ok(ResponseHelper.Success(data));
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUsuario([FromBody] UpdateUsuarioDTO usuario)
        {
            var data = await _userService.UpdateUsuarioAsync(usuario);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPost("estado/{id}")]
        public async Task<IActionResult> CambiarEstadoUsuario(int id,[FromBody] CambiarEstadoDTO cambio)
        {
            var desactivar = await _userService.CambiarEstadoUsuario(id,cambio.activo);
            return Ok(ResponseHelper.Success(desactivar));
        }

    }
}
