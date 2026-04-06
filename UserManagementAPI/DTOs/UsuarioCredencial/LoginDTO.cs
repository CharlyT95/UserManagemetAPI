using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial
{
    public class LoginDTO
    { 

        [Required(ErrorMessage = "Debe ingresar nombre de usuario")]
        public string UsuarioLogin { get; set; }

        [Required(ErrorMessage = "Debe ingresar contraseña")]
        public string Password { get; set; }
    }
}
