using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Permiso
{
    public class UpdatePermisoDTO
    {
        [Required(ErrorMessage = "El Id del permiso es obligatorio")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "El Código del permiso es obligatorio")]
        public string CodigoPermiso { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        public string? Modulo { get; set; }

        [Required(ErrorMessage = "usuario quien modifica es obligatorio")]
        public string? UsuarioModificacion { get; set; }
    }
}
