namespace Aduanas.Aci.Audit.Api.DTOs
{
    /// <summary>
    /// DTO de entrada — lo que recibe el endpoint desde los otros microservicios.
    /// No expone IdLog ni FechaEvento (se asignan internamente).
    /// </summary>
    public class AuditEventDto
    {
        public int UsuarioId { get; set; }
        public string Modulo { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public string TipoAccion { get; set; } = string.Empty;
        public string Tabla { get; set; } = string.Empty;
        public string RegistroId { get; set; } = string.Empty;
        public string? Peticion { get; set; }
        public string? Respuesta { get; set; }
        public string? ValorAnterior { get; set; }
        public string? ValorNuevo { get; set; }
        public string? Referencia { get; set; }
        public string? DireccionIP { get; set; }
    }

    /// <summary>
    /// DTO de salida — lo que devuelve el endpoint al confirmar el registro.
    /// </summary>
    public class AuditEventResponseDto
    {
        public long IdLog { get; set; }
        public int UsuarioId { get; set; }
        public string? Modulo { get; set; }
        public string? Servicio { get; set; }
        public string? TipoAccion { get; set; }
        public string? Tabla { get; set; }
        public string? RegistroId { get; set; }
        public DateTime FechaEvento { get; set; }
        public string? DireccionIP { get; set; }
    }
}
