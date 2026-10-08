using System.Security.Cryptography;

namespace LaudaryMis.Helpers
{
    // Random passwords the admin hands out (no look-alike characters such as 0/O or 1/l/I)
    public static class PasswordGenerator
    {
        private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Lower = "abcdefghijkmnpqrstuvwxyz";
        private const string Digits = "23456789";
        private const string Symbols = "@#$%&*";

        public static string Generate(int length = 10)
        {
            var all = Upper + Lower + Digits + Symbols;
            var chars = new List<char>
            {
                Pick(Upper), Pick(Lower), Pick(Digits), Pick(Symbols)
            };
            while (chars.Count < length)
                chars.Add(Pick(all));

            // Shuffle so the guaranteed characters are not always first
            for (int i = chars.Count - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }
            return new string(chars.ToArray());
        }

        private static char Pick(string set) => set[RandomNumberGenerator.GetInt32(set.Length)];
    }
}
