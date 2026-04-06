namespace Aduanas.Aci.Usuarios.Api.Services.Interfaces
{
    public interface IPasswordService
    {
        void CreatePassword(string password, out string hash, out string salt);
        bool VerifyPassword(string password, string passwordHash, string passwordSalt);
    }
}
