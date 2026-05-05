using Aduanas.Aci.Audit.Api.Common;
using Aduanas.Aci.Audit.Api.Data;
using Aduanas.Aci.Audit.Api.DTOs;
using Aduanas.Aci.Audit.Api.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aduanas.Aci.Audit.Api.Services
{
    public class AuditLogService
    {
        private readonly AuditoriaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<AuditEventDto> _validator;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService(
            AuditoriaDbContext context,
            IMapper mapper,
            IValidator<AuditEventDto> validator,
            ILogger<AuditLogService> logger)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ApiResponse<AuditEventResponseDto>> GuardarAsync(AuditEventDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);

            var usuario = await _context.Usuario.AnyAsync(u => u.IdUsuario == dto.IdUsuario && u.Activo);
            if (!usuario)
                return ApiResponse<AuditEventResponseDto>.Fail("Usuario no encontrado.");

            var log = _mapper.Map<AuditoriaLog>(dto);
            log.FechaEvento = DateTime.Now;
            log.TipoAccion = dto.TipoAccion.ToUpper();

            _context.AuditoriaLogs.Add(log);
            await _context.SaveChangesAsync();

            return ApiResponse<AuditEventResponseDto>.Ok(
                _mapper.Map<AuditEventResponseDto>(log),
                "Evento registrado correctamente."
            );
        }

        //public async Task<List<AuditEventResponseDto>> GuardarLoteAsync(List<AuditEventDto> dtos)
        //{
        //    var erroresGlobales = new List<string>();

        //    for (int i = 0; i < dtos.Count; i++)
        //    {
        //        var resultado = await _validator.ValidateAsync(dtos[i]);
        //        if (!resultado.IsValid)
        //        {
        //            var errores = string.Join(" | ", resultado.Errors.Select(e => e.ErrorMessage));
        //            erroresGlobales.Add($"[Evento {i + 1}]: {errores}");
        //        }
        //    }

        //    if (erroresGlobales.Count > 0)
        //        throw new ValidationException(string.Join(" || ", erroresGlobales));

        //    var logs = dtos.Select(dto =>
        //    {
        //        var log = _mapper.Map<AuditoriaLog>(dto);
        //        log.FechaEvento = DateTime.UtcNow;
        //        return log;
        //    }).ToList();

        //    _context.AuditoriaLogs.AddRange(logs);
        //    await _context.SaveChangesAsync();

        //    _logger.LogInformation("Lote de {Count} logs registrados.", logs.Count);

        //    return _mapper.Map<List<AuditEventResponseDto>>(logs);
        //}
    }
}
