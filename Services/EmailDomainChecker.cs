using DnsClient;
using Microsoft.Extensions.Caching.Memory;

namespace LaudaryMis.Services
{
    // Asks DNS whether an email domain exists and can receive mail, so addresses such as
    // name@gmail.commm or name@abcxyz123.in are refused. A lookup that fails for network reasons
    // returns null ("could not check"): a DNS outage must never stop a real person registering.
    public class EmailDomainChecker
    {
        private readonly LookupClient _dns;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EmailDomainChecker> _log;

        public EmailDomainChecker(IMemoryCache cache, ILogger<EmailDomainChecker> log)
        {
            _cache = cache;
            _log = log;
            _dns = new LookupClient(new LookupClientOptions
            {
                Timeout = TimeSpan.FromSeconds(3),
                Retries = 1,
                UseCache = true,
                ThrowDnsErrors = false
            });
        }

        // true: the domain takes mail. false: it does not exist / takes no mail. null: could not be checked.
        public async Task<bool?> AcceptsMailAsync(string domain)
        {
            domain = domain.Trim().TrimEnd('.').ToLowerInvariant();
            if (domain.Length == 0) return false;

            var key = "mail-domain:" + domain;
            if (_cache.TryGetValue(key, out bool known)) return known;

            try
            {
                var mx = await _dns.QueryAsync(domain, QueryType.MX);
                bool? result;

                if (mx.Header.ResponseCode == DnsHeaderResponseCode.NotExistentDomain)
                    result = false;
                // An MX of "." (RFC 7505) means the domain says it takes no mail at all
                else if (mx.Answers.MxRecords().Any(r => r.Exchange.Value.Trim('.').Length > 0))
                    result = true;
                else
                {
                    // No MX: mail can still be delivered to the domain's own address (A / AAAA)
                    var a = await _dns.QueryAsync(domain, QueryType.A);
                    var aaaa = await _dns.QueryAsync(domain, QueryType.AAAA);
                    result = a.Answers.ARecords().Any() || aaaa.Answers.AaaaRecords().Any();
                }

                _cache.Set(key, result.Value, TimeSpan.FromHours(6));
                return result;
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Could not check the mail domain {Domain}; allowing it.", domain);
                return null;
            }
        }
    }
}
