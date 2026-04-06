using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol;
using AutoMapper;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Mappings
{
    public class UsuarioRolProfile : Profile
    {
        public UsuarioRolProfile()
        {
            CreateMap<UsuarioRol, UsuarioRolDTO>();
            CreateMap<CreateUsuarioRolDTO, UsuarioRol>();
            CreateMap<UpdateUsuarioRolDTO, UsuarioRol>()
                .ForMember(p => p.IdUsuarioRol, opt => opt.Ignore())
                .ForMember(p => p.FechaCreacion, opt => opt.Ignore())
                .ForMember(p => p.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(p => p.Activo, opt => opt.Ignore());
            CreateMap<DeactivateUsuarioRolDTO, UsuarioRol>()
                .ForMember(p => p.IdUsuarioRol, opt => opt.Ignore())
                .ForMember(p => p.FechaCreacion, opt => opt.Ignore())
                .ForMember(p => p.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(p => p.FechaModificacion, opt => opt.Ignore())
                .ForMember(p => p.UsuarioModificacion, opt => opt.Ignore());
        }
    }
}
