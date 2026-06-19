namespace LaudaryMis.ViewModels
{
    public class PendingLinenReportVM
    {
        public int PickupId { get; set; }

        public string PickupNo { get; set; }

        public string HospitalName { get; set; }

        public string WardName { get; set; }

        public DateTime PickupDate { get; set; }

        public int TotalCollectedQty { get; set; }

        public int TotalDeliveredQty { get; set; }

        public int PendingQty { get; set; }

        public int PendingDays { get; set; }

        public string Status { get; set; }
    }
}