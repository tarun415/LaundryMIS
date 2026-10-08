namespace LaudaryMis.ViewModels
{
    public class OptionVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // One row of the admin's "CMS Accounts" page: a hospital and whether its CMS has a login yet
    public class CmsAccountVM
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public bool HasAccount { get; set; }
        public bool IsActive { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
