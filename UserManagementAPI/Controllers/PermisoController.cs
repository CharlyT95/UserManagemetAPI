using Aduanas.Aci.Usuarios.Api.DTOs;
using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.DTOs.Permiso;
using UserManagementAPI.Helpers;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisoController : ControllerBase
    {
        private readonly PermisoService _permisoService;

        public PermisoController(PermisoService permisoService)
        {
            _permisoService = permisoService;
        }

        [HttpGet("ListaPermisos")]
        public async Task<IActionResult> GetPermissions()
        {
            var data = await _permisoService.GetPermisos();

            return Ok(ResponseHelper.Success(
                data,
                data.Count == 0 ? "No hay datos que mostrar" : "Datos obtenidos correctamente"
            ));

        }

        [HttpGet("PermisoById")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var data = await _permisoService.GetPermisoById(id);
            return Ok(ResponseHelper.Success(
                data,
                data == null ? "No existe permiso que mostrar" : "Permiso obtenido correctamente"
            ));
        }

        [HttpPost("CrearPermiso")]
        public async Task<IActionResult> CreatePermiso([FromBody] CreatePermisoDTO permiso)
        {
            var data = await _permisoService.CreatePermisoAsync(permiso);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPut("ActualizarPermiso")]
        public async Task<IActionResult> UpdatePermiso([FromBody] UpdatePermisoDTO permiso)
        {
            var data = await _permisoService.UpdatePermiso(permiso);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPatch("CambiarEstado")]
        public async Task<IActionResult> DesactivarPermiso(int id, [FromBody] CambiarEstadoDTO cambio)
        {
            var desactivar = await _permisoService.DeactivatePermisoAsync(id,cambio.activo);
            return Ok(ResponseHelper.Success(desactivar));
        }



    }
}
