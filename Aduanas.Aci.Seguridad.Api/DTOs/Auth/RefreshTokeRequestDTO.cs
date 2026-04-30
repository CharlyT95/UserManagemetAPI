using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Seguridad.Api.DTOs.Auth
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
