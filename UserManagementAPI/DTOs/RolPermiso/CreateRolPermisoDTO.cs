using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso
{
    public class CreateRolPermisoDTO
    {
        [Required(ErrorMessage = "Es necesario el rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Es necesario el permiso a asociar")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "Es necesario quien crea el rolPermiso")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario que crea el registro no puede exceder 50 caracteres")]
        public string UsuarioCreacion { get; set; } = "admin";
    }
}
