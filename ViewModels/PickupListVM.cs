namespace LaudaryMis.ViewModels
{
    public class PickupListVM
    {
        public int RowNum { get; set; }

        public int PickupId { get; set; }

        public string PickupNo { get; set; }

        public DateTime PickupDateTime { get; set; }

        public int HospitalId { get; set; }

        public string HospitalName { get; set; }

        public int WardId { get; set; }

        public string WardName { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public string ShiftName { get; set; }

        public int TotalCollectedQty { get; set; }

        public int TotalDeliveredQty { get; set; }

        public int TotalPendingQty { get; set; }

        public bool IsInfected { get; set; }

        public string PickupBy { get; set; }

        public string ReceivedBy { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class PickupItemListVM
    {
        public int PickupItemId { get; set; }

        public int PickupId { get; set; }

        public int LinenTypeId { get; set; }

        public string LinenTypeName { get; set; }

        public int CollectedQty { get; set; }
        public int DeliveredQty { get; set; }
        public int PendingQty { get; set; }
    }
}