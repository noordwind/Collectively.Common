using System;
using System.Security.Cryptography;
using System.Text;

namespace Coolector.Common.Encryption
{
    public class Encrypter : IEncrypter
    {
        private const int MinSaltSize = 8;
        private const int MaxSaltSize = 12;
        private const int MinSecureKeySize = 40;
        private const int MaxSecureKeySize = 60;
        private static readonly Random Random = new Random();

        public string GetRandomSecureKey()
        {
            var size = Random.Next(MinSecureKeySize, MaxSecureKeySize);
            var bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }

        public string GetSalt(string value)
        {
            var random = new Random();
            var saltSize = random.Next(MinSaltSize, MaxSaltSize);
            var saltBytes = new byte[saltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public string GetHash(string value, string salt)
        {
            using (var sha512 = SHA512.Create())
            {
                var bytes = Encoding.Unicode.GetBytes(value + salt);
                var hash = sha512.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}