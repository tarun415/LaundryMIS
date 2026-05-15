namespace LaudaryMis.Models
{
    public class WeeklyVerificationModel
    {
        public int HospitalId { get; set; }
        public int TotalPickupQty { get; set; }
        public int TotalDeliveredQty { get; set; }
        public int TotalPendingQty { get; set; }
        public string Status { get; set; }
        public int WeekNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
        public string LogBookPath { get; set; }

        public string EntryIds { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public IFormFile? LogBookFile { get; set; }
    }
}
