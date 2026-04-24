using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class WPRVM
    {
        public int AgreementId { get; set; }

        // ✅ REQUIRED FOR UI
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public string? StaffName { get; set; }
        public string? Remarks { get; set; }

        public List<WPRParameter> Parameters { get; set; } = new();
        public List<WPRDetail> Details { get; set; } = new();

        public List<AgreementVM> Agreements { get; set; } = new();

        public int TotalScore { get; set; }

        public int ParameterCount => Parameters?.Count ?? 0;
    }
}