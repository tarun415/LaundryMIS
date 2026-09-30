using System.Collections.Concurrent;

namespace LaudaryMis.Helpers
{
    // In-memory brute-force guard. Counts are lost on app restart, which is
    // acceptable for a single-instance deployment.
    public class LoginAttemptTracker
    {
        private const int MaxAccountFailures = 5;
        private const int MaxIpFailures = 20;
        private static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(15);

        private class Entry
        {
            public int Failures;
            public DateTime LockedUntil;
            public DateTime LastFailure;
        }

        private readonly ConcurrentDictionary<string, Entry> _entries = new();

        public bool IsLocked(string account, string ip, out TimeSpan remaining)
        {
            remaining = TimeSpan.Zero;
            foreach (var key in new[] { "acc:" + account, "ip:" + ip })
            {
                if (_entries.TryGetValue(key, out var e))
                {
                    lock (e)
                    {
                        var left = e.LockedUntil - DateTime.UtcNow;
                        if (left > remaining) remaining = left;
                    }
                }
            }
            return remaining > TimeSpan.Zero;
        }

        public void RecordFailure(string account, string ip)
        {
            Register("acc:" + account, MaxAccountFailures);
            Register("ip:" + ip, MaxIpFailures);
            Cleanup();
        }

        public void Reset(string account) => _entries.TryRemove("acc:" + account, out _);

        private void Register(string key, int max)
        {
            var e = _entries.GetOrAdd(key, _ => new Entry());
            lock (e)
            {
                // Old failures expire so stray typos do not add up forever.
                if (DateTime.UtcNow - e.LastFailure > LockDuration) e.Failures = 0;
                e.Failures++;
                e.LastFailure = DateTime.UtcNow;
                if (e.Failures >= max)
                {
                    e.LockedUntil = DateTime.UtcNow + LockDuration;
                    e.Failures = 0;
                }
            }
        }

        private void Cleanup()
        {
            if (_entries.Count < 5000) return;
            foreach (var kv in _entries)
                if (DateTime.UtcNow - kv.Value.LastFailure > LockDuration
                    && kv.Value.LockedUntil < DateTime.UtcNow)
                    _entries.TryRemove(kv.Key, out _);
        }
    }
}
