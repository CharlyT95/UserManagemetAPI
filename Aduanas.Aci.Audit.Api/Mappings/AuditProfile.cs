using Aduanas.Aci.Audit.Api.DTOs;
using Aduanas.Aci.Audit.Api.Models;
using AutoMapper;

namespace Aduanas.Aci.Audit.Api.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditEventDto, AuditoriaLog>()
                .ForMember(dest => dest.IdLog, opt => opt.Ignore())
                .ForMember(dest => dest.FechaEvento, opt => opt.Ignore());
            CreateMap<AuditoriaLog, AuditEventResponseDto>();
        }
    }
}
