using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Rol
{
    public class DeactivateRol
    {
        [Required(ErrorMessage = "El Id de rol es obligatorio")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre de quien modifica es obligatorio")]
        public string? UsuarioModificacion { get; set; }
    }
}
