using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace FormsCreator.Infrastructure.Utils
{
    internal static class HashUtils
    {
        public static (string password, string salt) HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return (string.Empty, string.Empty);

            var rndNumber = RandomNumberGenerator.GetInt32(int.MaxValue);
            byte[] saltBytes = UniqueSalt(password + rndNumber);

            string hashPass = Convert.ToBase64String(UsePbkdf2(password, saltBytes));
            return (hashPass, Convert.ToBase64String(saltBytes));
        }

        private static byte[] UniqueSalt(string password)
        {
            byte[] passBytes = Encoding.Unicode.GetBytes(password);
            return UsePbkdf2(password, passBytes);
        }

        public static bool CheckHash(string attemptedPassword, string hash, string salt)
        {
            if (IsAllNull(attemptedPassword, hash, salt)) return true;
            if (string.IsNullOrWhiteSpace(attemptedPassword)) return false;
            string hashed = Convert.ToBase64String(UsePbkdf2(attemptedPassword, Convert.FromBase64String(salt)));
            return hashed == hash;
        }

        static bool IsAllNull(string? attemptedPass, string? hash, string? salt)
            => string.IsNullOrWhiteSpace(attemptedPass) && string.IsNullOrWhiteSpace(hash) && string.IsNullOrWhiteSpace(salt);

        private static byte[] UsePbkdf2(string password, byte[] saltBytes)
            => KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 150000,
                numBytesRequested: 512 / 8);
    }
}
