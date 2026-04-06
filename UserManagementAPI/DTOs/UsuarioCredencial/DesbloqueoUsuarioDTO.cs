using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial
{
    public class DesbloqueoUsuarioDTO
    {
        [Required(ErrorMessage = "Debe ingresar usuario a desbloquear")]
        public int IdUsuario { get; set; }
    }
}
