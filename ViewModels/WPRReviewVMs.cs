namespace LaudaryMis.ViewModels
{
    public static class WprStatus
    {
        public const string Pending = "Pending";
        public const string Verified = "Verified";
    }

    // One WPR in a list (CMS queue, hospital's own list, vendor's status list)
    public class WprListItemVM
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string? ProviderName { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalScore { get; set; }
        public int PaymentPercentage { get; set; }
        public string Status { get; set; } = WprStatus.Pending;
        public DateTime SubmittedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? LastEditedAt { get; set; }

        public string MonthLabel => Month is >= 1 and <= 12
            ? new DateTime(Year, Month, 1).ToString("MMMM yyyy")
            : $"{Month}/{Year}";
    }

    public class WprScoreVM
    {
        public int ParameterId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class WprEditLogVM
    {
        public string ParameterName { get; set; } = string.Empty;
        public int? OldScore { get; set; }
        public int NewScore { get; set; }
        public int? OldTotal { get; set; }
        public int NewTotal { get; set; }
        public string EditedByName { get; set; } = string.Empty;
        public string EditedByRole { get; set; } = string.Empty;
        public DateTime EditedAt { get; set; }
        public string? Remarks { get; set; }
    }

    // The review / edit page of one WPR
    public class WprReviewVM
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string? ProviderName { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalScore { get; set; }
        public int PaymentPercentage { get; set; }
        public string Status { get; set; } = WprStatus.Pending;
        public string? HospitalRemarks { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public List<WprScoreVM> Scores { get; set; } = new();
        public List<WprEditLogVM> EditLog { get; set; } = new();

        public bool IsPending => Status == WprStatus.Pending;

        public string MonthLabel => Month is >= 1 and <= 12
            ? new DateTime(Year, Month, 1).ToString("MMMM yyyy")
            : $"{Month}/{Year}";
    }

    // Posted back from the review page
    public class WprReviewPost
    {
        public int Id { get; set; }
        public List<WprScoreVM> Scores { get; set; } = new();
        public string? EditRemarks { get; set; }

        // "save" = keep the edits and leave it pending, "verify" = save the edits and verify
        public string Action { get; set; } = "save";
    }

    public class CmsDashboardVM
    {
        public string HospitalName { get; set; } = string.Empty;
        public int Pending { get; set; }
        public int Verified { get; set; }
        public int OpenDisputes { get; set; }
        public List<WprListItemVM> RecentPending { get; set; } = new();
    }
}
