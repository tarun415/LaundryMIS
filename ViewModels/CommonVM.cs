namespace LaudaryMis.ViewModels
{
    public class CommonVM
    {
        public class DropdownVM
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
        public class GetAgreementByHospitalVM
        {
            public int AgreementId { get; set; }

            public int ProviderId { get; set; }

            public DateTime AgreementStartDate { get; set; }

            public DateTime AgreementEndDate { get; set; }
        }
    }
}
