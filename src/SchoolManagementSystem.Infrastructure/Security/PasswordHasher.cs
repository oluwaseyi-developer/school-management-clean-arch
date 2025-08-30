using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using System.Security.Cryptography;


namespace SchoolManagementSystem.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;
        public void CreatePassword(string password, out byte[] hash, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(SaltSize);
            using var pbkbf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            hash = pbkbf2.GetBytes(KeySize);
        }

        public bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var pbkbf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var computed = pbkbf2.GetBytes(KeySize);
            return CryptographicOperations.FixedTimeEquals(computed, hash);
        }
    }
}
