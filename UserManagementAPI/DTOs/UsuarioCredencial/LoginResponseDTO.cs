using UserManagementAPI.DTOs.Permiso;
using UserManagementAPI.DTOs.Rol;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial
{
    public class LoginResponseDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string UsuarioLogin { get; set; }

        public List<LoginResponseRolesDTO> Roles { get; set; }
    }
}
