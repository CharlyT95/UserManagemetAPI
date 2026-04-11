using Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso;
using AutoMapper;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Mappings
{
    public class RolPermisoProfile : Profile
    {
        public RolPermisoProfile()
        {
            CreateMap<RolPermiso, RolPermisoDTO>();
            CreateMap<CreateRolPermisoDTO, RolPermiso>();
            CreateMap<UpdateRolPermiso, RolPermiso>()
                .ForMember(p => p.IdRolPermiso, opt => opt.Ignore())
                .ForMember(p => p.FechaCreacion, opt => opt.Ignore())
                .ForMember(p => p.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(p => p.Activo, opt => opt.Ignore());
        }
    }
}
