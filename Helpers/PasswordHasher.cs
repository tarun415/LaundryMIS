namespace LaudaryMis.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        // Legacy rows still hold plain-text passwords; they are accepted once
        // and re-hashed by the login code (see NeedsRehash).
        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrEmpty(stored)) return false;
            if (!IsHash(stored)) return stored == password;
            try { return BCrypt.Net.BCrypt.Verify(password, stored); }
            catch { return false; }
        }

        public static bool NeedsRehash(string stored) => !IsHash(stored);

        private static bool IsHash(string s) =>
            s.Length == 60 && (s.StartsWith("$2a$") || s.StartsWith("$2b$") || s.StartsWith("$2y$"));
    }
}
