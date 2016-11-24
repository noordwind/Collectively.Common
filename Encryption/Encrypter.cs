using System;
using System.Security.Cryptography;
using System.Text;

namespace Coolector.Common.Encryption
{
    public class Encrypter : IEncrypter
    {
        private static readonly string[] InvalidCharacters =
        {
            "+", "/", "\\", "=", "-", "_", "&", "?", ",", ".", ";", " ", "<", ">", "~", "!",
            ":", "'", "\"", "[", "]", "{", "}", "|", "%", "#", "$", "^", "8", "(", ")"
        };

        private static readonly int MinSaltSize = 8;
        private static readonly int MaxSaltSize = 12;
        private static readonly int MinSecureKeySize = 40;
        private static readonly int MaxSecureKeySize = 60;
        private static readonly Random Random = new Random();

        public string GetRandomSecureKey()
        {
            var size = Random.Next(MinSecureKeySize, MaxSecureKeySize);
            var bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                var base64String = Convert.ToBase64String(bytes);
                var stringBuilder = new StringBuilder(base64String);
                foreach (var invalidCharacter in InvalidCharacters)
                {
                    stringBuilder.Replace(invalidCharacter, string.Empty);
                }

                return stringBuilder.ToString();
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