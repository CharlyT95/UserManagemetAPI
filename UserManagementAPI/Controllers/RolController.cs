using Aduanas.Aci.Usuarios.Api.DTOs;
using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.DTOs.Rol;
using UserManagementAPI.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolController(RolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var data = await _rolService.getRoles();

            return Ok(ResponseHelper.Success(
                data,
                data.Count == 0 ? "No hay datos que mostrar" : "Datos obtenidos correctamente"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RolById(int id)
        {
            var data = await _rolService.getRolById(id);
            return Ok(ResponseHelper.Success(
            data,
            data == null ? "No hay datos que mostrar" : "Datos obtenidos correctamente"
           ));

        }

        [HttpPost]
        public async Task<IActionResult> CreateRol([FromBody] CreateRolDTO rol)
        {
            var data = await _rolService.CreateRol(rol);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol([FromBody] UpdateRolDTO rol)
        {
            var data = await _rolService.UpdateRol(rol);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPost("estado/{id}")]
        public async Task<IActionResult> CambiarEstadoRol(int id, [FromBody] CambiarEstadoDTO cambio)
        {
            var data = await _rolService.CambiarEstadoRol(id, cambio.activo);
            return Ok(ResponseHelper.Success(data));
        }

    }
}
