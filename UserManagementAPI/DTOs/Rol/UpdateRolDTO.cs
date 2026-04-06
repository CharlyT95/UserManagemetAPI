using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Rol
{
    public class UpdateRolDTO
    {
        [Required(ErrorMessage = "Es necesario el Id del Rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Es necesario el nombre del Rol")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es necesario la descripción del Rol")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Es necesario conocer quien modifica el rol")]
        public string UsuarioModificacion { get; set; }
    }
}
