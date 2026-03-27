using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Permiso
{
    public class DeactivatePermisoDTO
    {
        [Required(ErrorMessage = "El Id del permiso es obligatorio")]
        public int IdPermiso { get; set; }
    }
}
