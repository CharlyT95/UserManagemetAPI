using System.Security.Cryptography;

namespace Aduanas.Aci.Seguridad.Api.Helpers;

public static class PasswordHelper
{
    private const int SaltSize = 32;
    private const int HashSize = 32;
    private const int Iterations = 100_000; // mismo valor que usaste al crear

    /// <summary>Crea hash + salt para una contraseña nueva</summary>
    public static (byte[] hash, byte[] salt, int iteraciones) CrearHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: HashSize);

        return (hash, salt, Iterations);
    }

    /// <summary>Verifica una contraseña contra el hash almacenado</summary>
    public static bool VerificarPassword(
        string password, byte[] storedHash, byte[] storedSalt, int iteraciones)
    {
        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: storedSalt,
            iterations: iteraciones,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: storedHash.Length);

        // Comparación en tiempo constante (evita timing attacks)
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}