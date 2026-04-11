namespace Aduanas.Aci.Usuarios.Api.Services.Interfaces
{
    public interface IPasswordService
    {
        void CreatePassword(string password, out byte[] hash, out byte[] salt);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
