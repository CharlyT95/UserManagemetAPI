using System.Net.NetworkInformation;

namespace Aduanas.Aci.Usuarios.Api.Errors.Usuario
{
    public class UsuarioErrors
    {
        public const string CorreoDuplicado = "El correo ya está registrado";
        public const string LoginUsuarioDuplicado = "El nombre de usuario ya existe";

        public const string UsuarioNoEncontrado = "El usuario no existe";
        public const string UsuarioInactivoBoolInactivo = "El usuario se encuentra inactivo";
        public const string UsuarioActivoBoolActivo = "El usuario se encuentra activo";
    }
}
