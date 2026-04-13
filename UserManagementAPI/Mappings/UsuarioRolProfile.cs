using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol;
using AutoMapper;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Mappings
{
    public class UsuarioRolProfile : Profile
    {
        public UsuarioRolProfile()
        {
            CreateMap<UsuarioRol, UsuarioRolDTO>()
                .ForMember(p => p.Rol,opt => opt.MapFrom(src => src.Rol.Nombre)); ;
            CreateMap<CreateUsuarioRolDTO, UsuarioRol>()
                .ForMember(p => p.IdUsuarioRol, opt => opt.Ignore());
            CreateMap<UpdateUsuarioRolDTO, UsuarioRol>()
                  .ForMember(dest => dest.IdUsuarioRol, opt => opt.Ignore())
                  .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
