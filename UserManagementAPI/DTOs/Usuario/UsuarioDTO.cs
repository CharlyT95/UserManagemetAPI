using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Usuario
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El Id del usuario es obligatorio")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UsuarioLogin { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; }

        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        public string CorreoElectronico { get; set; }

    }
}
