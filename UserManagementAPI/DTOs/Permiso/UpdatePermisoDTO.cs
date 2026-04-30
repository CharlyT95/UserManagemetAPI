using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Permiso
{
    public class UpdatePermisoDTO
    {
        [Required(ErrorMessage = "El Id del permiso es obligatorio")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "El Código del permiso es obligatorio")]
        [MaxLength(50, ErrorMessage = "El código de permiso no puede exceder 50 caracteres")]
        public string CodigoPermiso { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        public string? Modulo { get; set; }

        [Required(ErrorMessage = "usuario quien modifica es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario que crea el registro no puede exceder 50 caracteres")]
        public string? UsuarioModificacion { get; set; } = "admin";

        public string? Accion { get; set; }

        [MaxLength(150, ErrorMessage = "La referencia no puede exceder 150 caracteres")]
        public string? Referencia { get; set; }
    }
}
