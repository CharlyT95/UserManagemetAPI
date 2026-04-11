using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class PasswordService : IPasswordService
    {

        public void CreatePassword(string password, out byte[] hash, out byte[] salt) 
        {
            using var hashmac = new HMACSHA512();
            var saltBytes = hashmac.Key;
            var hashBytes = hashmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            salt = hashmac.Key;
            hash = hashmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        }

        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hashmac = new HMACSHA512(passwordSalt);
            var computedHash = hashmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return CryptographicOperations.FixedTimeEquals(passwordHash, computedHash);
        }
    }
}
