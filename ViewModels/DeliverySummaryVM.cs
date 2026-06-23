namespace LaudaryMis.ViewModels
{
    public class DeliverySummaryVM
    {
        public int RowNum { get; set; }

        public int PickupId { get; set; }

        public string PickupNo { get; set; }

        public string HospitalName { get; set; }

        public string WardName { get; set; }

        public int TotalCollectedQty { get; set; }

        public int TotalDeliveredQty { get; set; }

        public int PendingQty { get; set; }

        public int DeliveryCount { get; set; }

        public DateTime? FirstDeliveryDate { get; set; }

        public DateTime? LastDeliveryDate { get; set; }

        public string DeliveryStatus { get; set; }
    }

    public class DeliveryHistoryVM
    {
        public string DeliveryNo { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public string LinenName { get; set; }

        public int DeliveredQty { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public bool IsVerified { get; set; }

        public int? VerifiedByUserId { get; set; }

        public DateTime? VerifiedDateTime { get; set; }
    }
}
