namespace Aduanas.Aci.Usuarios.Api.Audit
{
    /// <summary>
    /// DTO que se encola internamente y se envía al MS.Auditoria.
    /// Copia esta clase en cada microservicio existente.
    /// Debe coincidir con AuditEventDto del MS.Auditoria.
    /// </summary>
    public class AuditEvent
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
}
