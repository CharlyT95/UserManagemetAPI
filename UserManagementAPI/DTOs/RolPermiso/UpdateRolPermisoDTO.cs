using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso
{
    public class UpdateRolPermisoDTO
    {
        [Required(ErrorMessage = "Se encesita conocer el Id de la relación Rol-Permiso a editar")]
        public int IdRolPermiso { get; set; }

        [Required(ErrorMessage = "Es necesario el rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Es necesario el permiso a asociar")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "Es necesario el usuario que modifica")]
        public string UsuarioModificacion { get; set; }
    }
}
