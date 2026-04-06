using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso
{
    public class DeactivateRolPermisoDTO
    {
        [Required(ErrorMessage = "Es necesario el Id del RolPErmiso")]
        public int IdRolPermiso { get; set; }
    }
}
