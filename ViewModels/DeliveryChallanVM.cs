namespace LaudaryMis.ViewModels
{
    public class DeliveryChallanVM
    {
        public int PickupId { get; set; }

        public string PickupNo { get; set; }

        public string DeliveredBy { get; set; }

        public string? DeliveredByPhone { get; set; }

        public string ReceivedBy { get; set; }

        public string? ReceivedByPhone { get; set; }

        public string Remarks { get; set; }

        public List<DeliveryChallanItemVM> Items { get; set; } = new();
    }
    public class DeliveryChallanItemVM
    {
        public int LinenTypeId { get; set; }

        public string LinenName { get; set; }

        public int CollectedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int PendingQty { get; set; }

        public int DeliveryQty { get; set; }
    }
   
        public class DeliveryListVM
        {
            public int RowNum { get; set; }

            public int DeliveryId { get; set; }

            public string DeliveryNo { get; set; }

            public string PickupNo { get; set; }

            public string HospitalName { get; set; }

            public string WardName { get; set; }

            public int TotalCollectedQty { get; set; }

            public int DeliveredQty { get; set; }

            public int PendingQty { get; set; }

            public string Status { get; set; }

            public DateTime DeliveryDateTime { get; set; }

            public string? DeliveredBy { get; set; }

            public string? DeliveredByPhone { get; set; }

            public string? ReceivedBy { get; set; }

            public string? ReceivedByPhone { get; set; }

            public string? ProviderName { get; set; }

            public string? ProviderPhone { get; set; }
        }
    
}
