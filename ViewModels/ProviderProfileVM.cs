using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    // Vendor profile — RFP Part I, Eligibility (2.3) aur Format 6.
    public class ProviderProfileVM
    {
        public int ProviderId { get; set; }

        // ── tbl_Providers ─────────────────────────────────────
        [Required(ErrorMessage = "Firm ka naam daalein.")]
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? FirmName { get; set; }

        [Required(ErrorMessage = "Contact person ka naam daalein.")]
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? ProviderName { get; set; }

        [Required(ErrorMessage = "Mobile number daalein.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "10 digit ka sahi mobile number daalein.")]
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

        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z][1-9A-Z]Z[0-9A-Z]$", ErrorMessage = "Sahi 15 character ka GSTIN daalein (jaise 09ABCDE1234F1Z5).")]
        public string? GSTNo { get; set; }

        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$", ErrorMessage = "Sahi PAN daalein (jaise ABCDE1234F).")]
        public string? PANNo { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9/\-]*$", ErrorMessage = "EPF number mein sirf letters, numbers, / aur - ho sakte hain.")]
        public string? EPFNo { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9/\-]*$", ErrorMessage = "ESI number mein sirf letters, numbers, / aur - ho sakte hain.")]
        public string? ESINo { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9/\-]*$", ErrorMessage = "MSME/Udyam number mein sirf letters, numbers, / aur - ho sakte hain.")]
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

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "10 digit ka sahi mobile number daalein.")]
        public string? ContractManagerPhone { get; set; }

        [Range(0, 60, ErrorMessage = "Experience 0 se 60 saal ke beech ho.")]
        public int? ContractManagerExpYears { get; set; }

        // ── Bank (payment ke liye) ────────────────────────────
        [StringLength(150)]
        [RegularExpression(RegisterVM.SafeNamePattern, ErrorMessage = RegisterVM.SafeNameMessage)]
        public string? BankAccountName { get; set; }

        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "Account number 9 se 18 digit ka ho.")]
        public string? BankAccountNo { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Sahi IFSC daalein (jaise SBIN0001234).")]
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
            ("Firm ka type", !string.IsNullOrWhiteSpace(LegalStatus)),
            ("Registration number", !string.IsNullOrWhiteSpace(RegistrationNo)),
            ("GSTIN", !string.IsNullOrWhiteSpace(GSTNo)),
            ("PAN", !string.IsNullOrWhiteSpace(PANNo)),
            ("Address", !string.IsNullOrWhiteSpace(Address)),
            ("Contract Manager", !string.IsNullOrWhiteSpace(ContractManagerName)),
            ("Bank account", !string.IsNullOrWhiteSpace(BankAccountNo) && !string.IsNullOrWhiteSpace(BankIFSC))
        };

        public IEnumerable<(DocumentTypeInfo Type, ProviderDocumentVM? Latest)> DocumentChecklist() =>
            DocumentTypes.All.Select(t => (t, Documents
                .Where(d => d.DocumentType == t.Name)
                .OrderByDescending(d => d.UploadedOn)
                .FirstOrDefault()));

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
            new("Registration Certificate", true,  false, "Company / firm registration (Companies Act ya relevant Act)"),
            new("GST Certificate",          true,  false, "GST registration certificate"),
            new("PAN Card",                 true,  false, "Firm ka PAN"),
            new("EPF Registration",         true,  false, "EPF registration"),
            new("ESI Registration",         true,  false, "ESI registration"),
            new("Labour Licence",           true,  true,  "Contract Labour (R&A) Act, 1970 — iske bina payment release nahi hogi"),
            new("Solvency Certificate",     true,  true,  "Bank se, kam se kam ₹10 lakh per Schedule"),
            new("Affidavit (Format 3)",     true,  false, "Blacklisting / conviction nahi hone ka notarised affidavit"),
            new("Power of Attorney (Format 2)", true, false, "Signatory ko authorise karne ka POA / board resolution"),
            new("Contract Manager Resume",  true,  false, "5 saal ka experience"),
            new("ITR / Audited Balance Sheet", false, false, "Pichhle 3 saal"),
            new("Cancelled Cheque",         false, false, "Bank account ki pushti ke liye"),
            new("MSME / NSIC Certificate",  false, true,  "Agar hai to (EMD chhoot ke liye)"),
            new("Other",                    false, false, "Koi aur document")
        };

        public static DocumentTypeInfo? Find(string? name) =>
            All.FirstOrDefault(t => t.Name == name);
    }
}
