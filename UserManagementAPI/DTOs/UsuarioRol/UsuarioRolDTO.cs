using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol
{
    public class UsuarioRolDTO
    {
        public int IdUsuarioRol { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }

    }
}
