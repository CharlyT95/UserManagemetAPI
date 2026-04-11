namespace Aduanas.Aci.Usuarios.Api.Errors.UsuarioCredencial
{
    public class UsuarioCredencialErrors
    {
        public const string UsuarioNoExistente = "El usuario no se encuentra registrado";
        public const string CredencialExistente = "El usuario ya posee credenciales";
        public const string PasswordIncorrecto = "La contraseña actual es incorrecta";
        public const string Bloqueo = "El usuario actualmente se encuentra bloqueado";
        public const string CredencialesIncorrectas = "Usuario o contraseña incorrecto";
        public const string BloqueoAutomatico = "Se ha bloqueado el usuario, contacte con administración";
    }
}
