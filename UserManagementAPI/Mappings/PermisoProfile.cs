using AutoMapper;
using UserManagementAPI.DTOs.Permiso;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mappings
{
    public class PermisoProfile : Profile
    {
        public PermisoProfile()
        {
            CreateMap<Permiso,PermisoDTO>();
            CreateMap<CreatePermisoDTO,Permiso>();
            CreateMap<DeactivatePermisoDTO, Permiso>()
             .ForMember(p => p.IdPermiso, opt => opt.Ignore())
             .ForMember(p => p.CodigoPermiso, opt => opt.Ignore())
             .ForMember(p => p.Descripcion, opt => opt.Ignore())
             .ForMember(p => p.Modulo, opt => opt.Ignore())
             .ForMember(p => p.FechaCreacion, opt => opt.Ignore())
             .ForMember(p => p.UsuarioCreacion, opt => opt.Ignore())
             .ForMember(p => p.UsuarioModificacion, opt => opt.Ignore())
             .ForMember(p => p.FechaModificacion, opt => opt.Ignore());
            CreateMap<UpdatePermisoDTO, Permiso>()
                .ForMember(p => p.IdPermiso, opt => opt.Ignore())
                .ForMember(p => p.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(p => p.FechaCreacion, opt => opt.Ignore())
                .ForMember(p => p.Activo, opt => opt.Ignore());
        }
    }
}
