namespace LaudaryMis.ViewModels
{
    public class DeliveryAgingReportVM
    {
        public int PickupId { get; set; }

        public string PickupNo { get; set; }

        public string HospitalName { get; set; }

        public string WardName { get; set; }

        public DateTime PickupDate { get; set; }

        public int CollectedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int PendingQty { get; set; }

        public int AgingDays { get; set; }

        public string AgingStatus { get; set; }
    }

    public class DeliveryAgingSummaryVM
    {
        public int TotalPendingPickups { get; set; }

        public int TotalPendingQty { get; set; }

        public int CriticalCount { get; set; }

        public int WarningCount { get; set; }

        public int NormalCount { get; set; }
    }

    public class DeliveryAgingReportPageVM
    {
        public DeliveryAgingSummaryVM Summary { get; set; }

        public List<DeliveryAgingReportVM> Details { get; set; }
            = new();
    }
    public class DeliveryAgingItemVM
    {
        public string LinenName { get; set; }

        public int CollectedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int PendingQty { get; set; }
    }
}
