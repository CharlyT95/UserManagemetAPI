using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial
{
    public class CambiarPasswordDTO
    {
        [Required(ErrorMessage = "Debe ingresar usuario a cambiar contraseña")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Es necesaria su contraseña actual")]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "Es necesaria su contraseña nueva")]
        public string PasswordNueva { get; set; }
    }
}
