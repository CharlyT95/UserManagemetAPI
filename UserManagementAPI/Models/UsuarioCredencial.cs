namespace UserManagementAPI.Models
{
    public class UsuarioCredencial
    {
        public int IdUsuarioCredencial { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Iteraciones { get; set; }
        public DateTime FechaUltimoCambio { get; set; }
        public int IntentosFallidos { get; set; }
        public bool BloqueoTemporal {  get; set; }
    }
}
