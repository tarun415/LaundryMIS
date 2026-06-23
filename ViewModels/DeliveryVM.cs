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
    public class PickupDeliveryHistoryVM
    {
        public int DeliveryId { get; set; }

        public string DeliveryNo { get; set; }
        public string LinenName { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public int CollectedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int PendingQty { get; set; }    

        public string Status { get; set; }

        public string Remarks { get; set; }

        public bool IsVerified { get; set; }

        public int? VerifiedByUserId { get; set; }

        public DateTime? VerifiedDateTime { get; set; }
    }

}
