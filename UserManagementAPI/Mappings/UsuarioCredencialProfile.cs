using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial;
using AutoMapper;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Mappings
{
    public class UsuarioCredencialProfile : Profile
    {
        public UsuarioCredencialProfile()
        {
            CreateMap<CreateUsuarioCredencialDTO,UsuarioCredencial>();
            CreateMap<CambiarPasswordDTO,UsuarioCredencial>();
            CreateMap<DesbloqueoUsuarioDTO,UsuarioCredencial>();
            CreateMap<DesbloqueoUsuarioDTO,UsuarioCredencial>();
            CreateMap<LoginDTO,UsuarioCredencial>();
            CreateMap<UsuarioCredencialResponseDTO,UsuarioCredencial>();
        }
    }
}
