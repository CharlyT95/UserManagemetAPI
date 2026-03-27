namespace UserManagementAPI.Models
{
    public class RolPermiso
    {
        public int IdRolPermiso { get; set; }
        public int IdRol {  get; set; }
        public Rol Rol { get; set; }

        public int IdPermiso { get; set; }
        public Permiso Permiso { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string UsuarioCreacion { get; set; }
    }
}
