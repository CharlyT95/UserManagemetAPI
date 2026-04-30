using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Rol
{
    public class UpdateRolDTO
    {
        [Required(ErrorMessage = "Es necesario el Id del Rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Es necesario el nombre del Rol")]
        [MaxLength(25, ErrorMessage = "El nombre no puede exceder 25 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es necesario la descripción del Rol")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Es necesario conocer quien modifica el rol")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario que modifica no puede exceder 50 caracteres")]
        public string UsuarioModificacion { get; set; } = "admin";
    }
}
