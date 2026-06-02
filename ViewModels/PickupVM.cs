using LaudaryMis.Models;
using static LaudaryMis.ViewModels.CommonVM;

namespace LaudaryMis.ViewModels
{
    public class PickupVM
    {
        public int PickupId { get; set; }

        public int AgreementId { get; set; }

        public int HospitalId { get; set; }

        public int ProviderId { get; set; }

        public int WardId { get; set; }

        public DateTime PickupDateTime { get; set; }

        public string ShiftName { get; set; }

        public string PickupBy { get; set; }

        public string ReceivedBy { get; set; }

        public string Remarks { get; set; }

        public int CreatedBy { get; set; }

        public bool IsInfected { get; set; }
        public List<PickupItemVM> Items { get; set; }
            = new();

        // Dropdowns

        public IEnumerable<DropdownVM> Hospitals { get; set; }

        public IEnumerable<DropdownVM> Wards { get; set; }

        public IEnumerable<DropdownVM> Providers { get; set; }

        public IEnumerable<LinenType> LinenTypes { get; set; }

        public string PickupNo { get; set; }
        public string HospitalName { get; set; }
        public string ProviderName { get; set; }
        public string WardName { get; set; }
    }
    public class PickupItemVM
    {
        public int LinenTypeId { get; set; }
        public string LinenTypeName { get; set; }
        public int CollectedQty { get; set; }
    }
    public class DbResponse
    {
        public int Flag { get; set; }

        public string Message { get; set; }

        public int PickupId { get; set; }
    }
}
