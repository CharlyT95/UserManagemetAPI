
namespace UserManagementAPI.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; }
        public string Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; }

    }
}
