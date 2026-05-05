namespace Aduanas.Aci.Audit.Api.Common.Exceptions
{
    /// <summary>
    /// Error de validación o regla de negocio (400 Bad Request).
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// Recurso no encontrado (404 Not Found).
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Error no autorizado (401 Unauthorized).
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
