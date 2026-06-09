namespace LaudaryMis.ViewModels
{
    public class MonthlyReportVM
    {
        public string MonthName { get; set; }

        public int TotalPickups { get; set; }

        public int CollectedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int PendingQty { get; set; }

        public int FullyDelivered { get; set; }

        public int PartialDelivered { get; set; }
    }

    public class MonthlyPickupDetailVM
    {
        public int PickupId { get; set; }

        public string PickupNo { get; set; }

        public string HospitalName { get; set; }

        public string WardName { get; set; }

        public int CollectedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int PendingQty { get; set; }

        public string Status { get; set; }
    }
}
