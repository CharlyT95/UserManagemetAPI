namespace Aduanas.Aci.Audit.Api.DTOs
{
    public class AuditEventDto
    {
        public int IdUsuario { get; set; }
        public string Modulo { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public string TipoAccion { get; set; } = string.Empty;
        public string Tabla { get; set; } = string.Empty;
        public string IdRegistro { get; set; } = string.Empty;
        public string? Peticion { get; set; }
        public string? Respuesta { get; set; }
        public string? ValorAnterior { get; set; }
        public string? ValorNuevo { get; set; }
        public string? Referencia { get; set; }
        public string? DireccionIP { get; set; }
    }

    public class AuditEventResponseDto
    {
        public long IdLog { get; set; }
        public int IdUsuario { get; set; }
        public string? Modulo { get; set; }
        public string? Servicio { get; set; }
        public string? TipoAccion { get; set; }
        public string? Tabla { get; set; }
        public string? IdRegistro { get; set; }
        public DateTime FechaEvento { get; set; }
        public string? DireccionIP { get; set; }
    }
}
