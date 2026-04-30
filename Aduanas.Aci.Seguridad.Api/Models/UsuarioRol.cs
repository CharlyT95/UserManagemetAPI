namespace UserManagementAPI.Models
{
    public class UsuarioRol
    {
        public int IdUsuarioRol {  get; set; } 
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } 

        public int IdRol { get; set; }
        public Rol Rol { get; set; }
 
    }
}
