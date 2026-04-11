using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial;
using AutoMapper;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Mappings
{
    public class UsuarioCredencialProfile : Profile
    {
        public UsuarioCredencialProfile()
        {
            CreateMap<UsuarioCredencial,CreateUsuarioCredencialDTO>().ReverseMap();
          //  CreateMap<UsuarioCredencial,CambiarPasswordDTO>().ReverseMap();
            CreateMap<DesbloqueoUsuarioDTO,UsuarioCredencial>();
            CreateMap<LoginDTO,UsuarioCredencial>();
        }
    }
}
