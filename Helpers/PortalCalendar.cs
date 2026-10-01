using System.Globalization;

namespace LaudaryMis.Helpers
{
    // Contracts started on 1 April 2026, but the portal went live later.
    // Months before the portal start were billed offline, so WPR, monthly
    // bills and payments can only be created from the start month onwards
    // (otherwise an offline-paid month could be billed again).
    // Configure with "Portal:BillingStartDate" in appsettings.json.
    public class PortalCalendar
    {
        public DateTime BillingStart { get; }

        public PortalCalendar(IConfiguration config)
        {
            var raw = config["Portal:BillingStartDate"];
            BillingStart = DateTime.TryParse(raw, CultureInfo.InvariantCulture,
                                             DateTimeStyles.None, out var d)
                ? new DateTime(d.Year, d.Month, 1)
                : new DateTime(2026, 11, 1);
        }

        public bool IsBeforeStart(int month, int year) =>
            month is < 1 or > 12 || new DateTime(year, month, 1) < BillingStart;

        public string StartLabel =>
            BillingStart.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

        public string BeforeStartMessage(string what) =>
            $"{what} cannot be created for a month before {StartLabel}. " +
            $"The portal went live in {StartLabel}; earlier months were handled offline.";
    }
}
