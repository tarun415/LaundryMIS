namespace LaudaryMis.ViewModels
{
    public class DailyEntryListVM
    {
        public int EntryId { get; set; }
        public DateTime EntryDate { get; set; }

        public string HospitalName { get; set; }
        public string WardName { get; set; }
        public string LinenTypeName { get; set; }

        public int TotalPickupQty { get; set; }
        public int CleanDeliveredQty { get; set; }

        public string Status { get; set; }
    }


}
