namespace UserManagementAPI.Models.Base
{
    public abstract class BaseModel
    {
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
