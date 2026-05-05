using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aduanas.Aci.Audit.Api.Models
{
    [Table("AuditoriaLog")]
    public class AuditoriaLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdLog { get; set; }

        public int IdUsuario { get; set; }

        [MaxLength(50)]
        public string? Modulo { get; set; }

        [MaxLength(100)]
        public string? Servicio { get; set; }

        [MaxLength(20)]
        public string? TipoAccion { get; set; }

        [MaxLength(100)]
        public string? Tabla { get; set; }

        [MaxLength(50)]
        public string? IdRegistro { get; set; }

        [MaxLength(2000)]
        public string? Peticion { get; set; }

        [MaxLength(2000)]
        public string? Respuesta { get; set; }

        public string? ValorAnterior { get; set; }   // nvarchar(max)
        public string? ValorNuevo { get; set; }       // nvarchar(max)

        [MaxLength(500)]
        public string? Referencia { get; set; }

        public DateTime FechaEvento { get; set; } = DateTime.UtcNow;

        [MaxLength(45)]
        public string? DireccionIP { get; set; }
    }
}
