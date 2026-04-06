using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Rol
{
    public class DeactivateRol
    {
        [Required(ErrorMessage = "El Id de rol es obligatorio")]
        public int IdRol { get; set; }
    }
}
