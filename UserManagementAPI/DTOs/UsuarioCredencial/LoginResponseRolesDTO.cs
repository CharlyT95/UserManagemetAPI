using UserManagementAPI.DTOs.Permiso;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial
{
    public class LoginResponseRolesDTO
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }

        public List<PermisoDTO> Permisos { get; set; }
    }
}
