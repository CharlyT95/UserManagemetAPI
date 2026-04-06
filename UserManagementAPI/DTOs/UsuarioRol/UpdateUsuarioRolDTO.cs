using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol
{
    public class UpdateUsuarioRolDTO
    {
        [Required(ErrorMessage = "El Id de asignación Usuario-Rol es obligatorio")]
        public int IdUsuarioRol { get; set; }

        [Required(ErrorMessage = "El Id de usuario es obligatorio")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El Id de Rol es obligatorio")]
        public int IdRol { get; set; }
    }
}
