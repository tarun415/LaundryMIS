namespace LaudaryMis.ViewModels
{
    public class DeliveryVM
    {
        public int EntryId { get; set; }
        public string? DeliveredBy { get; set; }
        public string? ReceivedBackBy { get; set; }
        public string? Remarks { get; set; }

        public List<DeliveryItemVM> Items { get; set; } = new();
    }

    public class DeliveryItemVM
    {
        public string? LinenType { get; set; }
        public int CleanCount { get; set; }
    }
}
