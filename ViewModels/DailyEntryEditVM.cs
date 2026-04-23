namespace LaudaryMis.ViewModels
{
    public class DailyEntryEditVM
    {
        public int EntryId { get; set; }

        public int DirtyQty { get; set; }
        public int SoiledQty { get; set; }
        public int InfectedQty { get; set; }

        public int TotalPickupQty { get; set; }

        public int CleanDeliveredQty { get; set; }

        public string? Remarks { get; set; }
        public int SubmittedBy { get; set; }
    }
}
