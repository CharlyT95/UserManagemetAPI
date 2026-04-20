using UserManagementAPI.Models.Base;

namespace UserManagementAPI.Models
{
    public class Permiso : BaseModel
    {
        public int IdPermiso { get; set; }
        public string CodigoPermiso { get; set; }
        public string Descripcion { get; set; }
        public string? Modulo { get; set; } 
        public string? Accion {  get; set; }
        public string? Referencia {  get; set; }

    }
}
