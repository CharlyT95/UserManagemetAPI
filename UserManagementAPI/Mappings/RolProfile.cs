using AutoMapper;
using UserManagementAPI.DTOs.Rol;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mappings
{
    public class RolProfile : Profile
    {
        public RolProfile()
        {
            CreateMap<Rol,RolDTO>();
            CreateMap<CreateRolDTO, Rol>();
            CreateMap<UpdateRolDTO, Rol>()
                .ForMember(r => r.IdRol, opt => opt.Ignore())
                .ForMember(r => r.FechaCreacion, opt => opt.Ignore())
                .ForMember(r => r.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(r => r.Activo, opt => opt.Ignore());
            CreateMap<DeactivateRol, Rol>()
                .ForMember(r => r.IdRol, opt => opt.Ignore())
                .ForMember(r => r.Nombre, opt => opt.Ignore())
                .ForMember(r => r.Descripcion, opt => opt.Ignore())
                .ForMember(r => r.FechaCreacion, opt => opt.Ignore())
                .ForMember(r => r.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(r => r.FechaModificacion, opt => opt.Ignore())
                .ForMember(r => r.UsuarioModificacion, opt => opt.Ignore());
        }
    }
}
