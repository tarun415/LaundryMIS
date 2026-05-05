namespace LaudaryMis.ViewModels
{
    public class BillListItemVM
    {
        public int BillId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
        public decimal WPRAvgScore { get; set; }
        public decimal PaymentBandPercent { get; set; }
        public decimal NetPayableAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsScoreOverridden { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProviderId { get; set; }    // ← ADD
        public string ProviderName { get; set; } = string.Empty;
        public string MonthLabel =>
            new DateTime(BillingYear, BillingMonth, 1).ToString("MMMM yyyy");

        public string StatusBadgeClass => Status switch
        {
            "Draft" => "badge-secondary",
            "HospitalSubmitted" => "badge-info",
            "HospitalApproved" => "badge-primary",
            "HospitalRejected" => "badge-warning",
            "CMSApproved" => "badge-success",
            "CMSRejected" => "badge-danger",
            _ => "badge-secondary"
        };
    }
}