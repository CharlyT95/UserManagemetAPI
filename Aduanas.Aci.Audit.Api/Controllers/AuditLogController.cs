using Aduanas.Aci.Audit.Api.Common;
using Aduanas.Aci.Audit.Api.DTOs;
using Aduanas.Aci.Audit.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Registra un evento de auditoría individual.
        /// POST /api/auditlog
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] AuditEventDto dto)
        {
            var resultado = await _service.GuardarAsync(dto);
            return Ok(ApiResponse<AuditEventResponseDto>.Ok(resultado, "Evento registrado correctamente."));
        }

        /// <summary>
        /// Registra múltiples eventos en un solo request.
        /// POST /api/auditlog/lote
        /// </summary>
        [HttpPost("lote")]
        public async Task<IActionResult> RegistrarLote([FromBody] List<AuditEventDto> dtos)
        {
            if (dtos is null || dtos.Count == 0)
                return BadRequest(ApiResponse<object>.Fail("La lista de eventos no puede estar vacía."));

            var resultados = await _service.GuardarLoteAsync(dtos);
            return Ok(ApiResponse<List<AuditEventResponseDto>>.Ok(
                resultados,
                $"{resultados.Count} eventos registrados correctamente."
            ));
        }

        /// <summary>
        /// Health check — verifica que el MS está activo.
        /// GET /api/auditlog/health
        /// </summary>
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(ApiResponse<object>.Ok(
                new { fecha = DateTime.UtcNow },
                "MS.Auditoria activo."
            ));
        }
    }
}
