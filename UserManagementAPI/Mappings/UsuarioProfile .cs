using AutoMapper;
using UserManagementAPI.DTOs.Usuario;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile() 
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<CreateUsuarioDTO, Usuario>();
            CreateMap<UpdateUsuarioDTO, Usuario>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore());
            CreateMap<DeactivateUsuarioDTO, Usuario>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioLogin, opt => opt.Ignore())
                .ForMember(dest => dest.Nombres, opt => opt.Ignore())
                .ForMember(dest => dest.Apellidos, opt => opt.Ignore())
                .ForMember(dest => dest.CorreoElectronico, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioModificacion, opt => opt.Ignore());
        }
        
    
    }
}
