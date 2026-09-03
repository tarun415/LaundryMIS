namespace LaudaryMis.Infrastructure
{
    /// <summary>
    /// Per-tab sign-in slots.
    ///
    /// A browser sends one cookie per origin, so a single auth cookie can only
    /// ever hold one signed-in user: signing in as a second role in another tab
    /// replaces it, and the first tab silently becomes that second user.
    ///
    /// Every URL is therefore scoped to a slot ("/u/0/...", "/u/1/...") and each
    /// slot has its own cookie whose Path is that prefix. The browser then only
    /// sends slot 0's cookie to "/u/0/..." requests, so tabs on different slots
    /// are genuinely independent sessions. This is the same approach Google uses
    /// for its "/u/0/" account switching.
    /// </summary>
    public static class TabSlots
    {
        /// <summary>How many accounts can be signed in at once in one browser.</summary>
        public const int Count = 4;

        public const string Prefix = "u";

        public const string SlotItemKey = "TabSlot";

        /// <summary>Default scheme; forwards to the current request's slot.</summary>
        public const string PolicySchemeName = "TabSlots";

        public static string SchemeFor(int slot) => $"TabSlot{slot}";

        public static string CookieNameFor(int slot) => $"LaundryMISAuth{slot}";

        /// <summary>Browser-visible path the slot's cookie is scoped to.</summary>
        public static string CookiePathFor(int slot) => $"/{Prefix}/{slot}";

        public static bool IsValid(int slot) => slot >= 0 && slot < Count;

        /// <summary>
        /// Splits "/u/2/Report/MonthlyReport" into slot 2 and "/Report/MonthlyReport".
        /// </summary>
        public static bool TrySplit(PathString path, out int slot, out PathString rest)
        {
            slot = 0;
            rest = path;

            if (!path.HasValue)
                return false;

            // Matched with StartsWithSegments rather than by splitting the
            // string, so segment boundaries and odd input (trailing or repeated
            // slashes) are handled by the framework.
            for (int candidate = 0; candidate < Count; candidate++)
            {
                var prefix = new PathString($"/{Prefix}/{candidate}");

                if (!path.StartsWithSegments(
                        prefix,
                        StringComparison.OrdinalIgnoreCase,
                        out var remaining))
                    continue;

                slot = candidate;
                rest = remaining.HasValue ? remaining : new PathString("/");

                return true;
            }

            return false;
        }

        /// <summary>The slot the current request is running under.</summary>
        public static int CurrentSlot(this HttpContext context)
        {
            return context.Items.TryGetValue(SlotItemKey, out var value)
                   && value is int slot
                ? slot
                : 0;
        }

        /// <summary>
        /// Names which slots are signed in. Each slot's auth cookie is scoped to
        /// that slot's path, so a request on "/u/0" never sees slot 1's cookie
        /// and cannot tell whether it is taken. This one small cookie is scoped
        /// to "/" purely so the account switcher can find a free slot. It holds
        /// slot numbers only - no identity, and it is not used to authenticate.
        /// </summary>
        public const string IndexCookieName = "LaundryMISTabs";

        public static HashSet<int> OccupiedSlots(HttpRequest request)
        {
            var occupied = new HashSet<int>();

            if (!request.Cookies.TryGetValue(IndexCookieName, out var value)
                || string.IsNullOrWhiteSpace(value))
                return occupied;

            foreach (var part in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(part, out var slot) && IsValid(slot))
                    occupied.Add(slot);
            }

            return occupied;
        }

        public static void MarkOccupied(HttpContext context, int slot, bool occupied)
        {
            var slots = OccupiedSlots(context.Request);

            if (occupied)
                slots.Add(slot);
            else
                slots.Remove(slot);

            if (slots.Count == 0)
            {
                context.Response.Cookies.Delete(IndexCookieName, new CookieOptions { Path = "/" });
                return;
            }

            context.Response.Cookies.Append(
                IndexCookieName,
                string.Join(',', slots.OrderBy(s => s)),
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });
        }

        /// <summary>
        /// Paths served as-is, without being pushed into a slot. Static assets
        /// are shared by every slot and carry no session.
        /// </summary>
        public static bool IsSlotless(PathString path)
        {
            string[] roots =
            {
                "/css", "/js", "/lib", "/images", "/img",
                "/WarningLetters", "/Rotativa", "/favicon.ico"
            };

            foreach (var root in roots)
            {
                if (path.StartsWithSegments(root, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
