using coursify_backend.Interfaces.IService;
using System.Security.Cryptography;

namespace coursify_backend.Services
{
    public class MiscService : IMiscService
    {
        private const int SaltSize = 128 / 8; // 16 bytes
        private const int KeySize = 256 / 8; // 32 bytes
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize); // RandomNumberGenerator.Create()
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string[] chunks = hashedPassword.Split(Delimiter);
            byte[] salt = Convert.FromBase64String(chunks[0]);
            byte[] actualHash = Convert.FromBase64String(chunks[1]);

            byte[] hashedInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithm, KeySize);

            return CryptographicOperations.FixedTimeEquals(actualHash, hashedInput);
        }
    }
}
