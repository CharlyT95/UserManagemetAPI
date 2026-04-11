using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial
{
    public class CreateUsuarioCredencialDTO
    {
        [Required(ErrorMessage = "Debe ingresar usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar contraseña")]
        public string Password { get; set; }
    }
}
