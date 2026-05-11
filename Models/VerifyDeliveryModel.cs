namespace LaudaryMis.Models
{
    public class VerifyDeliveryModel
    {
        public int DeliveryId { get; set; }

        public int VerifiedQty { get; set; }

        public string LogBookPath { get; set; }

        public int VerifiedBy { get; set; }

        public string? VerificationRemark { get; set; }
    }
}