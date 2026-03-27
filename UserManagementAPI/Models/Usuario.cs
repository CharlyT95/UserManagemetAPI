using UserManagementAPI.Models.Base;

namespace UserManagementAPI.Models
{
    public class Usuario : BaseModel
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; }
        public string Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string CorreoElectronico { get; set; }

    }
}
