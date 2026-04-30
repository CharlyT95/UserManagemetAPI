using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Rol
{
    public class CreateRolDTO
    {
        [Required(ErrorMessage = "El nombre de rol es obligatorio")]
        [MaxLength(25, ErrorMessage = "El nombre no puede exceder 25 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del rol es obligatorio")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage ="Es necesario quien crea el rol")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario que crea el registro no puede exceder 50 caracteres")]
        public string UsuarioCreacion { get; set; } = "admin";
    }
}
