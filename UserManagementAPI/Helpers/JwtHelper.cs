using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Aduanas.Aci.Usuarios.Api.Helpers
{
    public class JwtHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        public JwtHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int ObtenerUsuarioId()
        {
            var claim = _httpContext.HttpContext?.User
                            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? "0";
            return int.TryParse(claim, out var id) ? id : 0;
        }
    }
}