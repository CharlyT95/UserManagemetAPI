using System.Threading.Channels;

namespace Aduanas.Aci.Usuarios.Api.Audit
{
    /// <summary>
    /// Inyecta este cliente en cualquier Service o Controller.
    /// Llama a .Registrar() — es fire & forget, no bloquea tu flujo.
    /// Copia esta clase en cada microservicio existente.
    /// </summary>
    public class AuditoriaClient
    {
        private readonly Channel<AuditEvent> _channel;
        private readonly ILogger<AuditoriaClient> _logger;

        public AuditoriaClient(Channel<AuditEvent> channel, ILogger<AuditoriaClient> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        /// <summary>
        /// Encola el evento. No lanza excepción ni bloquea.
        /// </summary>
        public void Registrar(AuditEvent evento)
        {
            if (!_channel.Writer.TryWrite(evento))
            {
                _logger.LogWarning(
                    "Cola de auditoría llena. Evento descartado: {Tabla}/{TipoAccion}",
                    evento.Tabla, evento.TipoAccion
                );
            }
        }
    }
}
