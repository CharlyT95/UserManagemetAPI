using Aduanas.Aci.Usuarios.Api.Audit;

namespace Aduanas.Aci.Usuarios.Api.Helpers
{
    public class RegistroAuditoria
    {
        private readonly AuditoriaClient _auditoria;
        private readonly IHttpContextAccessor _httpContext;
        private readonly JwtHelper _jwtHelper;

        public RegistroAuditoria(AuditoriaClient auditoria, IHttpContextAccessor httpContext, JwtHelper jwtHelper)
        {
            _auditoria = auditoria;
            _httpContext = httpContext;
            _jwtHelper = jwtHelper;
        }

        public void RegistrarAudit(string servicio, string tipoAccion, string tabla, string registro, string valorAnterior, string valorNuevo)
        {
            _auditoria.Registrar(new AuditEvent
            {
                IdUsuario = _jwtHelper.ObtenerUsuarioId(),
                Modulo = "GestionUsuarios",
                Servicio = servicio,
                TipoAccion = tipoAccion,
                Tabla = tabla,
                IdRegistro = registro,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                DireccionIP = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString()
            });
        }
    }
}
