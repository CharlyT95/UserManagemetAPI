using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol
{
    public class DeactivateUsuarioRolDTO
    {
        [Required(ErrorMessage = "Se necesita el Id de la asignación")]
        public int IdUsuarioRol { get; set; }

        //[Required(ErrorMessage = "Es necesario el valor de activación")]
        //public bool Activo { get; set; }
    }
}
