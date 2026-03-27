using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Rol
{
    public class CreateRolDTO
    {
        [Required(ErrorMessage = "El nombre de rol es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del rol es obligatorio")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage ="Es necesario quien crea el rol")]
        public string UsuarioCreacion { get; set; }
    }
}
