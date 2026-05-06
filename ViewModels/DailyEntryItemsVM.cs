namespace LaudaryMis.ViewModels
{
    public class DailyEntryItemsVM
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public string? LinenTypeName { get; set; }
        public int TotalPickupQty { get; set; }
        public int CleanDeliveredQty { get; set; }


    }
}
