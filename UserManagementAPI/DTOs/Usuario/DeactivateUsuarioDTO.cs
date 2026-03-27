using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Usuario
{
    public class DeactivateUsuarioDTO
    {
        [Required(ErrorMessage = "El Id de usuario es obligatorio")]
        public int IdUsuario { get; set; }
    }
}
