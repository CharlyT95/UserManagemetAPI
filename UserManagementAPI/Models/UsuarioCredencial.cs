namespace UserManagementAPI.Models
{
    public class UsuarioCredencial
    {
        public int IdUsuarioCredencial { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int Iteraciones { get; set; }
        public DateTime FechaUltimoCambios { get; set; }
        public int IntentosFallidos { get; set; }
        public bool BloqueoTemporal {  get; set; }
    }
}
