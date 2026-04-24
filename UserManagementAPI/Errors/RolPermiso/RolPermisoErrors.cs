namespace Aduanas.Aci.Usuarios.Api.Errors.RolPermiso
{
    public class RolPermisoErrors
    {
        public const string RolNoEncontrado = "Rol inexistente";
        public const string PermisoNoEncontrado = "Permiso inexistente";
        public const string RolPermisoNoEncontrado = "Asignacion inexistente";
        public const string RolSinPermisos = "Rol no tiene permisos asignados";
        public const string PermisoYaAsignado = "El permiso ya se encuentra asignado";


        public const string InactivoBoolInactivo = "La asignacion  ya se encuentra inactivo";
        public const string ActivoBoolActivo = "La asignacion ya se encuentra activo";
    }
}
