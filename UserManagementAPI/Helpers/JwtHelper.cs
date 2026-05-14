using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

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
            var claim = _httpContext.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
         ?? _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
         ?? _httpContext.HttpContext?.User?.FindFirst("sub")?.Value
         ?? "0";


            return int.TryParse(claim, out var id) ? id : 0;
        }
    }
}