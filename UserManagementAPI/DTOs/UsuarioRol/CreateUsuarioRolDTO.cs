using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol
{
    public class CreateUsuarioRolDTO
    {
        [Required(ErrorMessage = "El Id de usuario es obligatorio")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El Id de Rol es obligatorio")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre de usuario quien crea este registro es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario que crea el registro no puede exceder 50 caracteres")]
        public string UsuarioCreacion { get; set; } = "admin";
    }
}
