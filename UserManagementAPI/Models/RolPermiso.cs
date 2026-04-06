using UserManagementAPI.Models.Base;

namespace UserManagementAPI.Models
{
    public class RolPermiso : BaseModel
    {
        public int IdRolPermiso { get; set; }
        public int IdRol {  get; set; }
        public Rol Rol { get; set; }

        public int IdPermiso { get; set; }
        public Permiso Permiso { get; set; }
    }
}
