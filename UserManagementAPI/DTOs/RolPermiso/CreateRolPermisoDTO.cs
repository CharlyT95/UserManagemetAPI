using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso
{
    public class CreateRolPermisoDTO
    {
        [Required(ErrorMessage = "Es necesario el rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Es necesario el permiso a asociar")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "Es necesario quien crea el rol")]
        public string UsuarioCreacion { get; set; }
    }
}
