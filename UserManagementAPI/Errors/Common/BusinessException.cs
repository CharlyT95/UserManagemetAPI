using Microsoft.AspNetCore.Connections.Features;

namespace Aduanas.Aci.Usuarios.Api.Errors.Common
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
