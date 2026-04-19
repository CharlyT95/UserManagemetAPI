using Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso;
using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Helpers;

namespace Aduanas.Aci.Usuarios.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolPermisoController : ControllerBase
    {

        private readonly RolPermisoService _rolPermisoService;

        public RolPermisoController(RolPermisoService rolPermisoService)
        {
            _rolPermisoService = rolPermisoService;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateRolPermisoDTO rolpermiso)
        {
            var data = await _rolPermisoService.AsignarRolPermiso(rolpermiso);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpPut]
        public async Task<IActionResult> Modificar([FromBody] UpdateRolPermisoDTO rolpermiso)
        {
            var data = await _rolPermisoService.ModificarRolPermiso(rolpermiso);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPermisos(int idRol)
        {
            var data = await _rolPermisoService.GetRolPermiso(idRol);
            return Ok(ResponseHelper.Success(data)); 
        }
    }
}
