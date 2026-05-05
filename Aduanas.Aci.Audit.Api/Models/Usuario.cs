using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Audit.Api.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public bool Activo {  get; set; }
    }
}
