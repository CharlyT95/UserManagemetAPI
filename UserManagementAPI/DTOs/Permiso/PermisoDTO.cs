using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs.Permiso
{
    public class PermisoDTO
    {
        [Required(ErrorMessage = "El ID del permiso es obligatorio")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "El Código del permiso es obligatorio")]
        public string CodigoPermiso { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }
        public string? Modulo { get; set; }
        public string? Accion { get; set; }
        public string? Referencia { get; set; }
    }
}
