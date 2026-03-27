using UserManagementAPI.Models.Base;

namespace UserManagementAPI.Models
{
    public class Rol : BaseModel
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
