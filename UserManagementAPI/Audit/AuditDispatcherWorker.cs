using System.Threading.Channels;

namespace Aduanas.Aci.Usuarios.Api.Audit
{
    /// <summary>
    /// Corre en background dentro de cada microservicio.
    /// Consume la cola interna y envía los eventos al MS.Auditoria por HTTP.
    /// Copia esta clase en cada microservicio existente.
    /// </summary>
    public class AuditDispatcherWorker : BackgroundService
    {
        private readonly Channel<AuditEvent> _channel;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuditDispatcherWorker> _logger;

        public AuditDispatcherWorker(
            Channel<AuditEvent> channel,
            IHttpClientFactory httpClientFactory,
            ILogger<AuditDispatcherWorker> logger)
        {
            _channel = channel;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AuditDispatcherWorker iniciado.");

            await foreach (var evento in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("AuditoriaClient");
                    var response = await client.PostAsJsonAsync("/api/auditlog", evento, stoppingToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(
                            "MS.Auditoria respondió {Status} para {Tabla}/{Accion}",
                            response.StatusCode, evento.Tabla, evento.TipoAccion
                        );
                    }
                }
                catch (Exception ex)
                {
                    // Nunca propaga — la auditoría no rompe el flujo principal
                    _logger.LogError(ex,
                        "Error enviando evento de auditoría: {Tabla}/{Accion}",
                        evento.Tabla, evento.TipoAccion
                    );
                }
            }
        }
    }
}
