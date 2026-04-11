namespace Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol
{
    public class UsuarioRolErrors
    {
        public const string RolYaAsignado = "El rol ya ha sido asignado al usuario";
        public const string UsuarioNoExiste = "El usuario no existe";
        public const string RolNoExiste = "El rol no existe o se encuentra inactivo";

        public const string UsuarioNull = "Usuario vacío o inexistente";
        public const string RolAsignadoNull = "Para desactivar un rol a un usuario es necesario especificar el rol";
        public const string RolInactivoBoolInactivo = "La asignación ya se encuentra inactiva";
        public const string RolActivoBoolActivo = "La asignación ya se encuentra activa";
        
    }
}
