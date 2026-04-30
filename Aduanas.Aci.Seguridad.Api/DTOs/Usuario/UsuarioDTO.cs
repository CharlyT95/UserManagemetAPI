namespace Aduanas.Aci.Seguridad.Api.DTOs.Usuario
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; } = string.Empty;
        public List<RolDTO> Roles { get; set; } = new();
        public List<string> Permisos { get; set; } = new();
    }
}
