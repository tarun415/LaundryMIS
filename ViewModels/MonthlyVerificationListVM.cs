namespace LaudaryMis.ViewModels
{
    public class MonthlyVerificationListVM
    {
        public int RowNum { get; set; }

        public string EntryIds { get; set; }

        public int WeekNo { get; set; }

        public string Status { get; set; }

        public DateTime EntryDate { get; set; }

        public string HospitalName { get; set; }

        public int TotalPickupQty { get; set; }

        public string Remark { get; set; }

        public string LogBookPath { get; set; }

        public int HospitalId { get; set; }

        public int CleanDeliveredQty { get; set; }

        public int TotalPendingQty { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
    }
   
}
