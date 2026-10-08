namespace LaudaryMis.ViewModels
{
    // One hospital + month in the service provider's "My Bills" list
    public class BillMonthVM
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }

        public int VerifiedWeeks { get; set; }
        public int PendingWeeks { get; set; }
        public decimal? AvgScore { get; set; }

        // Filled in by the service
        public int ExpectedWeeks { get; set; }
        public decimal? NetPayableAmount { get; set; }
        public int Version { get; set; } = 1;
        public bool CurrentVersionPrinted { get; set; }
        public DateTime? LastPrintedAt { get; set; }
        public bool HasOpenDispute { get; set; }

        // Every week of the month is reported and CMS-verified
        public bool IsReady => PendingWeeks == 0 && VerifiedWeeks >= ExpectedWeeks && ExpectedWeeks > 0;

        public string MonthLabel => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
    }

    // The bill that is shown and printed
    public class BillVM
    {
        public int AgreementId { get; set; }
        public int HospitalId { get; set; }
        public int ProviderId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string? District { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public string? ContractNo { get; set; }

        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
        public int Version { get; set; } = 1;
        public DateTime PrintedAt { get; set; } = DateTime.Now;

        public int SanctionedBeds { get; set; }
        public decimal RatePerBedPerYear { get; set; }
        public decimal GSTPercent { get; set; } = 18m;

        public decimal WPRAvgScore { get; set; }
        public int WPRWeeksFound { get; set; }

        public decimal AnnualValueExGST { get; set; }
        public decimal AnnualValueInGST { get; set; }
        public decimal MonthlyGrossAmount { get; set; }
        public decimal PaymentBandPercent { get; set; }
        public decimal BasePayableAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TDSAmount { get; set; }
        public decimal NetPayableAmount { get; set; }

        public List<WprListItemVM> Weeks { get; set; } = new();

        public string MonthName => new DateTime(BillingYear, BillingMonth, 1).ToString("MMMM yyyy");
    }

    // The agreement figures a bill is calculated from
    public class BillAgreementInfo
    {
        public int AgreementId { get; set; }
        public int HospitalId { get; set; }
        public int ProviderId { get; set; }
        public int SanctionedBeds { get; set; }
        public decimal RatePerBedPerYear { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string? District { get; set; }
        public string? ContractNo { get; set; }
        public string ProviderName { get; set; } = string.Empty;
    }

    public class BillPrintLogVM
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        public int Version { get; set; }
        public decimal WPRAvgScore { get; set; }
        public decimal NetPayableAmount { get; set; }
        public DateTime PrintedAt { get; set; }

        public string MonthLabel => new DateTime(BillYear, BillMonth, 1).ToString("MMMM yyyy");
    }

    // Resolved / open dispute counts of one hospital + month (drives the bill version)
    public class DisputeCountVM
    {
        public int HospitalId { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        public int Resolved { get; set; }
        public int Opened { get; set; }
    }
}
