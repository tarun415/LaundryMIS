using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class DailyEntryVM
    {
        public DateTime EntryDate { get; set; }

        public string? Ward { get; set; }
        public string? Shift { get; set; }

        public string? CollectedBy { get; set; }
        public string? ReceivedBy { get; set; }
        public string? Supervisor { get; set; }

        public int ProviderId { get; set; }
        public int HospitalId { get; set; }

        public string? Status { get; set; }

        public bool IsInfected { get; set; }   // ✅ ADD
        public string? Remarks { get; set; }   // ✅ ADD
        public List<LinenItemVM> Items { get; set; } = new();
        public List<Hospital> Hospitals { get; set; } = new();

        // 🔥 ADD THIS (IMPORTANT)
        public List<LinenType> LinenTypes { get; set; } = new();
    }

    public class LinenItemVM
    {
        public string? LinenType { get; set; }
        public int DirtyCount { get; set; }
    }
}