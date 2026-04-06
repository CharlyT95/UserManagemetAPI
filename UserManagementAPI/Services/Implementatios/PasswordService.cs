using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class PasswordService : IPasswordService
    {
        public void CreatePassword(string password, out string hash, out string salt) 
        {
            using var hashmac = new HMACSHA512();
            var saltBytes = hashmac.Key;
            var hashBytes = hashmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            salt = Convert.ToBase64String(saltBytes);
            hash = Convert.ToBase64String(hashBytes);

        }

        public bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            var saltBytes = Convert.FromBase64String(passwordSalt);

            using var hashmac = new HMACSHA512(saltBytes);

            var transHash = hashmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(passwordHash), transHash);
        }
    }
}
