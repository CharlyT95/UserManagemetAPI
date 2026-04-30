using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Usuario
{
    public class CreateUsuarioDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
        public string UsuarioLogin { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombres { get; set; }

        [MaxLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [MaxLength(150, ErrorMessage = "El correo eléctronico no puede exceder 150 caracteres")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El nombre de usuario quien crea este registro es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario que crea el registro no puede exceder 50 caracteres")]
        public string UsuarioCreacion { get; set; } = "admin";
    }
}
