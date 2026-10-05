using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    // Vendor profile — RFP Part I, Eligibility (2.3) aur Format 6.
    public class ProviderProfileVM
    {
        public int ProviderId { get; set; }

        // ── tbl_Providers ─────────────────────────────────────
        [Required(ErrorMessage = "Enter the firm name.")]
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? FirmName { get; set; }

        [Required(ErrorMessage = "Enter the contact person's name.")]
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? ProviderName { get; set; }

        [Required(ErrorMessage = "Enter the mobile number.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string? Phone { get; set; }

        // Login email — sirf dikhane ke liye
        public string? Email { get; set; }

        // ── Firm (Format 6, Part 2) ───────────────────────────
        public string? LegalStatus { get; set; }

        [StringLength(100)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? RegistrationNo { get; set; }

        [StringLength(200)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? RegistrationAuthority { get; set; }

        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Enter a valid 15-character GSTIN (for example 09ABCDE1234F1Z5).")]
        public string? GSTNo { get; set; }

        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$", ErrorMessage = "Enter a valid PAN (for example ABCDE1234F).")]
        public string? PANNo { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9/\-]*$", ErrorMessage = "The EPF number may only contain letters, numbers, / and -.")]
        public string? EPFNo { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9/\-]*$", ErrorMessage = "The ESI number may only contain letters, numbers, / and -.")]
        public string? ESINo { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9/\-]*$", ErrorMessage = "The MSME/Udyam number may only contain letters, numbers, / and -.")]
        public string? MSMENo { get; set; }

        [StringLength(500)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? Address { get; set; }

        [StringLength(100)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? ContactDesignation { get; set; }

        // ── Contract Manager (RFP 2.3-V, Article 9) ───────────
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? ContractManagerName { get; set; }

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string? ContractManagerPhone { get; set; }

        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years.")]
        public int? ContractManagerExpYears { get; set; }

        // ── Bank (payment ke liye) ────────────────────────────
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? BankAccountName { get; set; }

        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "The account number must be 9 to 18 digits.")]
        public string? BankAccountNo { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Enter a valid IFSC (for example SBIN0001234).")]
        public string? BankIFSC { get; set; }

        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? BankName { get; set; }

        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? BankBranch { get; set; }

        public DateTime? UpdatedOn { get; set; }

        // ── Page data (form se nahi aata) ─────────────────────
        public List<ProviderDocumentVM> Documents { get; set; } = new();

        public bool ReadOnly { get; set; }

        public static readonly string[] LegalStatuses =
        {
            "Private Limited Company", "Public Limited Company", "LLP",
            "Partnership Firm", "Proprietorship", "Society", "Trust", "Section 8 Company"
        };

        // Profile ke zaroori fields (completeness ke liye)
        public IEnumerable<(string Label, bool Done)> RequiredFields() => new[]
        {
            ("Firm type", !string.IsNullOrWhiteSpace(LegalStatus)),
            ("Registration number", !string.IsNullOrWhiteSpace(RegistrationNo)),
            ("GSTIN", !string.IsNullOrWhiteSpace(GSTNo)),
            ("PAN", !string.IsNullOrWhiteSpace(PANNo)),
            ("Address", !string.IsNullOrWhiteSpace(Address)),
            ("Contract Manager", !string.IsNullOrWhiteSpace(ContractManagerName)),
            ("Bank account", !string.IsNullOrWhiteSpace(BankAccountNo) && !string.IsNullOrWhiteSpace(BankIFSC))
        };

        public IEnumerable<(DocumentTypeInfo Type, ProviderDocumentVM? Latest)> DocumentChecklist() =>
            DocumentTypes.All.Select(t => (t, LatestOf(t)));

        // Documents that do not expire never show a validity date, even if one was stored
        private ProviderDocumentVM? LatestOf(DocumentTypeInfo type)
        {
            var latest = Documents
                .Where(d => d.DocumentType == type.Name)
                .OrderByDescending(d => d.UploadedOn)
                .FirstOrDefault();
            if (latest != null && !type.HasExpiry)
                latest.ValidTill = null;
            return latest;
        }

        public int CompletionPercent
        {
            get
            {
                var fields = RequiredFields().ToList();
                var docs = DocumentChecklist().Where(c => c.Type.Required).ToList();
                int total = fields.Count + docs.Count;
                int done = fields.Count(f => f.Done) + docs.Count(d => d.Latest != null && !d.Latest.IsExpired);
                return total == 0 ? 0 : (int)Math.Round(done * 100.0 / total);
            }
        }
    }

    public class ProviderDocumentVM
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string? DocumentNo { get; set; }
        public DateTime? ValidTill { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? OriginalFileName { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        public DateTime UploadedOn { get; set; }

        public bool IsExpired => ValidTill.HasValue && ValidTill.Value.Date < DateTime.Today;
        public bool ExpiresSoon => ValidTill.HasValue && !IsExpired && ValidTill.Value.Date <= DateTime.Today.AddDays(30);
    }

    public record DocumentTypeInfo(string Name, bool Required, bool HasExpiry, string Hint);

    // RFP Part I 2.3 / Format 9 Compliance Matrix aur Part III General Terms (9)
    public static class DocumentTypes
    {
        public static readonly DocumentTypeInfo[] All =
        {
            new("Registration Certificate", true,  false, "Company / firm registration (Companies Act or the relevant Act)"),
            new("GST Certificate",          true,  false, "GST registration certificate"),
            new("PAN Card",                 true,  false, "The firm's PAN"),
            new("EPF Registration",         true,  false, "EPF registration"),
            new("ESI Registration",         true,  false, "ESI registration"),
            new("Labour Licence",           false, false, "Contract Labour (R&A) Act, 1970 — optional (used in tender evaluation)"),
            new("Solvency Certificate",     true,  true,  "From the bank, at least ₹10 lakh per Schedule"),
            new("Affidavit (Format 3)",     true,  false, "Notarised affidavit that the firm is not blacklisted or convicted"),
            new("Power of Attorney (Format 2)", true, false, "POA / board resolution authorising the signatory"),
            new("Contract Manager Resume",  true,  false, "5 years of experience"),
            new("ITR / Audited Balance Sheet", false, false, "Last 3 years"),
            new("Cancelled Cheque",         false, false, "To confirm the bank account"),
            new("MSME / NSIC Certificate",  false, true,  "If available (for the EMD exemption)"),
            new("Other",                    false, false, "Any other document")
        };

        public static DocumentTypeInfo? Find(string? name) =>
            All.FirstOrDefault(t => t.Name == name);
    }
}
