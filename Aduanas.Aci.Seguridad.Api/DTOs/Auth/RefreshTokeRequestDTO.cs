using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Seguridad.Api.DTOs.Auth;

public class RefreshTokenRequestDTO
{
    [Required(ErrorMessage = "El AccessToken es requerido")]
    public string AccessToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "El RefreshToken es requerido")]
    public string RefreshToken { get; set; } = string.Empty;
}
