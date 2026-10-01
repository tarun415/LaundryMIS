using System.Security.Cryptography;
using System.Text;

namespace LaudaryMis.Helpers
{
    // Activation codes look like "K7M49-QXD2P": 10 characters from an alphabet
    // without look-alikes (no 0/O, 1/I/L), about 50 bits of randomness.
    public static class ActivationCodeHelper
    {
        private const string Alphabet = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";   // 31 characters
        private const int Length = 10;

        public static string Generate()
        {
            var chars = new char[Length];
            for (int i = 0; i < Length; i++)
                chars[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];

            return new string(chars, 0, 5) + "-" + new string(chars, 5, 5);
        }

        // Users type codes in lower case, with spaces or without the dash.
        public static string Normalize(string? code) =>
            new string((code ?? "").Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();

        public static string Hash(string? code) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Normalize(code)))).ToLowerInvariant();

        public static bool LooksValid(string? code) =>
            Normalize(code).Length == Length;
    }
}
