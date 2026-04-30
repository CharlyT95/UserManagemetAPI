using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aduanas.Aci.Seguridad.Api.Models;

[Table("RefreshTokens")]
public class RefreshToken
{
    [Key]
    public int IdRefreshToken { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario? Usuario { get; set; }

    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; } = false;
    public string? CreatedByIp { get; set; }
}