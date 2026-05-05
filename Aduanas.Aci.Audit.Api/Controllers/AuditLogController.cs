using Aduanas.Aci.Audit.Api.Common;
using Aduanas.Aci.Audit.Api.DTOs;
using Aduanas.Aci.Audit.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Aduanas.Aci.Audit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class AuditLogController : ControllerBase
    {
        private readonly AuditLogService _service;

        public AuditLogController(AuditLogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] AuditEventDto dto)
        {
            var resultado = await _service.GuardarAsync(dto);
            return Ok(resultado);
        }

        /// <summary>
        /// Registra múltiples eventos en una sola petición.
        /// POST /api/auditlog/lote
        /// </summary>
        //[HttpPost("lote")]
        //public async Task<IActionResult> RegistrarLote([FromBody] List<AuditEventDto> dtos)
        //{
        //    if (dtos is null || dtos.Count == 0)
        //        return BadRequest(ApiResponse<object>.Fail("La lista de eventos no puede estar vacía."));

        //    var resultados = await _service.GuardarLoteAsync(dtos);
        //    return Ok(ApiResponse<List<AuditEventResponseDto>>.Ok(
        //        resultados,
        //        $"{resultados.Count} eventos registrados correctamente."
        //    ));
        //}

        [HttpGet("activo")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(ApiResponse<object>.Ok(
                new { fecha = DateTime.UtcNow }, Assembly.GetExecutingAssembly().GetName().Name + 
            " activo."
            ));
        }
    }
}
