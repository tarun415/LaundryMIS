using LaudaryMis.Models;
using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    public class MonthlyBillVM
    {
        // ── Identifiers ────────────────────────────────────────
        public int? BillId { get; set; }
        public int AgreementId { get; set; }
        public int HospitalId { get; set; }
        public int ProviderId { get; set; }

        [Required]
        [Range(1, 12)]
        public int BillingMonth { get; set; }

        [Required]
        public int BillingYear { get; set; }

        // ── Agreement Info ─────────────────────────────────────
        public string HospitalName { get; set; } = "";
        public string ContractNo { get; set; } = "";
        public string ProviderName { get; set; } = "";
        public int SanctionedBeds { get; set; }
        public decimal RatePerBedPerYear { get; set; }
        public decimal GSTPercent { get; set; } = 18;

        // ── WPR ────────────────────────────────────────────────
        public decimal? AutoCalculatedScore { get; set; }
        public int WPRWeeksFound { get; set; }

        [Range(0, 100)]
        public decimal WPRAvgScore { get; set; }

        public bool IsScoreOverridden { get; set; }
        public string? OverrideReason { get; set; }

        // ── Deductions ─────────────────────────────────────────
        public decimal AdditionalDeductions { get; set; }
        public string? DeductionRemarks { get; set; }

        // ── Amounts ────────────────────────────────────────────
        public decimal AnnualValueExGST { get; set; }
        public decimal AnnualValueInGST { get; set; }
        public decimal MonthlyGrossAmount { get; set; }
        public decimal PaymentBandPercent { get; set; }
        public decimal BasePayableAmount { get; set; }
        public decimal TDSAmount { get; set; }
        public decimal NetPayableAmount { get; set; }

        // ── STATUS FLOW ─────────────────────────────────────────
        public string Status { get; set; } = "Draft";

        public string StatusDisplay => Status switch
        {
            "Draft" => "✏️ Draft",
            "HospitalSubmitted" => "📤 Hospital Ko Bheja",
            "HospitalApproved" => "✅ Hospital Approved",
            "HospitalRejected" => "❌ Hospital Rejected",
            "CMSApproved" => "✅ CMS Approved",
            "CMSRejected" => "❌ CMS Rejected",
            _ => Status
        };

        // ── BUTTON FLAGS ───────────────────────────────────────

        // Provider
        public bool CanSubmitToHospital =>
            Status == "Draft" || Status == "HospitalRejected";

        public bool CanSubmit => CanSubmitToHospital; // 🔥 FIX

        public bool CanEdit =>
            Status == "Draft" || Status == "HospitalRejected";

        // Hospital
        public bool CanHospitalAction =>
            Status == "HospitalSubmitted";

        // CMS
        public bool CanCMSAction =>
            Status == "HospitalApproved";

        // ── Workflow ───────────────────────────────────────────
        public List<BillWorkflowLogVM> WorkflowLog { get; set; } = new();

        public string MonthName =>
            new DateTime(BillingYear, BillingMonth, 1).ToString("MMMM yyyy");
    }
}