using System.Text.RegularExpressions;

namespace LaudaryMis.Helpers
{
    // Decides whether a mobile number or an email address looks real. Nothing is sent to the
    // number or the address, so these rules cannot prove who owns it; they only stop values
    // that are obviously made up (9999999999, test@gmail.com, name@gmail.commm ...).
    public static class ContactRules
    {
        public const string MobileMessage =
            "Enter your real 10-digit mobile number. A placeholder such as 9999999999 or 9876543210 is not accepted.";

        // The format is 10 digits starting 6-9 (checked by the form too). Returns a message when the
        // number is not in that format or is an obvious placeholder, otherwise null.
        public static string? MobileProblem(string? mobile)
        {
            var m = (mobile ?? "").Trim();
            if (!Regex.IsMatch(m, @"^[6-9]\d{9}$"))
                return "Enter a valid 10-digit mobile number.";
            return IsPlaceholderMobile(m) ? MobileMessage : null;
        }

        private static bool IsPlaceholderMobile(string m)
        {
            if (m.Distinct().Count() <= 3) return true;                  // 9898989898, 9999900000
            if (m.EndsWith("000000")) return true;                       // 9810000000
            if (LongestChain(m, (a, b) => a == b) >= 6) return true;     // 6 or more equal digits in a row
            if (LongestChain(m, (a, b) => (b - a + 10) % 10 == 1) >= 6) return true;   // 123456, 890123
            if (LongestChain(m, (a, b) => (a - b + 10) % 10 == 1) >= 6) return true;   // 654321, 321098
            foreach (var period in new[] { 2, 3, 5 })                    // 9879879879, 9876598765
                if (IsRepeating(m, period)) return true;
            return false;
        }

        // Longest run of consecutive digits where every neighbouring pair satisfies `linked`
        private static int LongestChain(string digits, Func<int, int, bool> linked)
        {
            int best = 1, current = 1;
            for (int i = 1; i < digits.Length; i++)
            {
                current = linked(digits[i - 1] - '0', digits[i] - '0') ? current + 1 : 1;
                best = Math.Max(best, current);
            }
            return best;
        }

        private static bool IsRepeating(string digits, int period)
        {
            for (int i = period; i < digits.Length; i++)
                if (digits[i] != digits[i - period]) return false;
            return true;
        }

        // ── Email ─────────────────────────────────────────────

        private static readonly Regex EmailPattern = new(
            @"^[A-Za-z0-9._%+\-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9\-]{0,61}[A-Za-z0-9])?\.)+[A-Za-z]{2,24}$",
            RegexOptions.Compiled);

        private static readonly HashSet<string> DisposableDomains = new(StringComparer.OrdinalIgnoreCase)
        {
            "mailinator.com", "guerrillamail.com", "guerrillamail.net", "guerrillamail.org", "sharklasers.com",
            "10minutemail.com", "10minutemail.net", "tempmail.com", "temp-mail.org", "temp-mail.io",
            "tempmailo.com", "tempinbox.com", "throwawaymail.com", "yopmail.com", "yopmail.net", "getnada.com",
            "nada.email", "trashmail.com", "trashmail.net", "maildrop.cc", "dispostable.com", "fakeinbox.com",
            "mintemail.com", "mytemp.email", "moakt.com", "emailondeck.com", "spamgourmet.com", "mailnesia.com",
            "burnermail.io", "discard.email", "mohmal.com", "mailcatch.com", "spambox.us", "fakemail.net"
        };

        // Common slips for the big providers; the message tells the user what was probably meant
        private static readonly Dictionary<string, string> DomainTypos = new(StringComparer.OrdinalIgnoreCase)
        {
            ["gmial.com"] = "gmail.com", ["gmai.com"] = "gmail.com", ["gamil.com"] = "gmail.com",
            ["gmaill.com"] = "gmail.com", ["gnail.com"] = "gmail.com", ["gmail.co"] = "gmail.com",
            ["gmail.con"] = "gmail.com", ["gmail.cm"] = "gmail.com", ["gmail.om"] = "gmail.com",
            ["yahooo.com"] = "yahoo.com", ["yaho.com"] = "yahoo.com", ["yahoo.con"] = "yahoo.com",
            ["hotmial.com"] = "hotmail.com", ["hotmai.com"] = "hotmail.com", ["hotmail.con"] = "hotmail.com",
            ["outlok.com"] = "outlook.com", ["outlook.con"] = "outlook.com"
        };

        // Local parts that are a sample rather than somebody's real address
        private static readonly HashSet<string> SampleLocalParts = new(StringComparer.OrdinalIgnoreCase)
        {
            "test", "testing", "test1", "demo", "sample", "example", "abc", "abcd", "xyz", "xxx",
            "noreply", "no-reply", "none", "null", "na", "nil", "asdf", "qwerty"
        };

        // Syntax and "does this look like somebody's real address" rules; the domain itself is
        // checked separately (see EmailDomainChecker). Returns a message, or null when it passes.
        public static string? EmailProblem(string? email)
        {
            var e = (email ?? "").Trim();
            if (e.Length == 0) return "Enter your email address.";
            if (e.Length > 200 || !EmailPattern.IsMatch(e)) return "Enter a valid email address.";

            var at = e.LastIndexOf('@');
            var local = e[..at];
            var domain = e[(at + 1)..].ToLowerInvariant();

            if (local.Length > 64 || local.StartsWith('.') || local.EndsWith('.') || local.Contains(".."))
                return "Enter a valid email address.";

            if (DisposableDomains.Contains(domain))
                return "Temporary / disposable email addresses are not accepted. Use your own email.";

            if (DomainTypos.TryGetValue(domain, out var fix))
                return $"Check the spelling of the email: did you mean @{fix}?";

            if (SampleLocalParts.Contains(local))
                return "Enter your own email address, not a sample one.";

            // Gmail only issues addresses of 6 to 30 letters, digits or dots
            if (domain is "gmail.com" or "googlemail.com")
            {
                var name = local.Split('+')[0];
                if (name.Length < 6 || name.Length > 30 || !Regex.IsMatch(name, @"^[A-Za-z0-9.]+$"))
                    return "This Gmail address cannot exist (Gmail addresses have 6 to 30 letters, digits or dots). Check the spelling.";
            }

            return null;
        }

        public static string DomainOf(string email) =>
            email[(email.LastIndexOf('@') + 1)..].Trim().ToLowerInvariant();

        // ── Sign-in identifier ────────────────────────────────

        // What a user typed in "Mobile number or Email": an email (lower-cased) or a 10-digit mobile.
        // Returns null when it is neither. An email is only matched, never judged: accounts created
        // before these rules existed must still be able to sign in.
        public static string? NormalizeLoginId(string? input, out bool isEmail)
        {
            isEmail = false;
            var s = (input ?? "").Trim();
            if (s.Length == 0 || s.Length > 200) return null;

            if (s.Contains('@'))
            {
                isEmail = true;
                return s.ToLowerInvariant();
            }

            if (s.Any(ch => !(char.IsDigit(ch) || ch is ' ' or '-' or '+'))) return null;

            var digits = new string(s.Where(char.IsDigit).ToArray());
            if (digits.Length == 12 && digits.StartsWith("91")) digits = digits[2..];      // +91 98765 43210
            else if (digits.Length == 11 && digits.StartsWith('0')) digits = digits[1..];  // 098765 43210

            // A placeholder number (0000000000 is stored on several old records) can never sign in
            return MobileProblem(digits) == null ? digits : null;
        }
    }
}
