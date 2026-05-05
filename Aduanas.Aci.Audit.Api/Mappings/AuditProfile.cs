using Aduanas.Aci.Audit.Api.DTOs;
using Aduanas.Aci.Audit.Api.Models;
using AutoMapper;

namespace Aduanas.Aci.Audit.Api.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            // AuditEventDto (entrada) → AuditoriaLog (entidad BD)
            // FechaEvento se asigna en el Service, no viene del DTO
            CreateMap<AuditEventDto, AuditoriaLog>()
                .ForMember(dest => dest.IdLog, opt => opt.Ignore())
                .ForMember(dest => dest.FechaEvento, opt => opt.Ignore());

            // AuditoriaLog (entidad BD) → AuditEventResponseDto (salida)
            CreateMap<AuditoriaLog, AuditEventResponseDto>();
        }
    }
}
