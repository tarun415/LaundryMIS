using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    public static class DisputeStatus
    {
        public const string Open = "Open";
        public const string Resolved = "Resolved";
        public const string Rejected = "Rejected";
    }

    public class DisputeListItemVM
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string? DistrictName { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public string Status { get; set; } = DisputeStatus.Open;
        public DateTime RaisedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolutionRemarks { get; set; }
        public string? LetterFile { get; set; }
        public string? LetterOriginalName { get; set; }

        public bool IsOpen => Status == DisputeStatus.Open;
        public bool HasLetter => !string.IsNullOrEmpty(LetterFile);
        public string MonthLabel => new DateTime(BillYear, BillMonth, 1).ToString("MMMM yyyy");

        public string StatusBadge => Status switch
        {
            DisputeStatus.Open => "badge-warn",
            DisputeStatus.Resolved => "badge-ok",
            DisputeStatus.Rejected => "badge-danger",
            _ => "badge-navy"
        };
    }

    // A month whose bill the vendor has printed and may dispute
    public class PrintedMonthVM
    {
        public int Month { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class RaiseDisputeVM
    {
        [Range(1, int.MaxValue, ErrorMessage = "Select a hospital.")]
        public int HospitalId { get; set; }

        [Range(2000, 2100, ErrorMessage = "Select a year.")]
        public int Year { get; set; }

        [Range(1, 12, ErrorMessage = "Select a month.")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Tell us what is wrong with the bill.")]
        [StringLength(1000)]
        public string Remarks { get; set; } = string.Empty;

        public List<OptionVM> Hospitals { get; set; } = new();
    }

    // The admin's page to fix the WPR behind a disputed bill
    public class DisputeResolveVM
    {
        public DisputeListItemVM Dispute { get; set; } = new();
        public List<WprReviewVM> Weeks { get; set; } = new();
    }

    public class DisputeResolvePost
    {
        public int DisputeId { get; set; }

        [Required(ErrorMessage = "Write what was corrected and why.")]
        [StringLength(1000)]
        public string Remarks { get; set; } = string.Empty;

        public IFormFile? Letter { get; set; }

        public List<WprReviewPost> Weeks { get; set; } = new();
    }
}
