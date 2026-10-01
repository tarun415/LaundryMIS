-- Signed-agreement details (Agreement No., Schedule, LOI, contract value, Performance Security).
-- All columns are nullable, so existing agreements keep working. Safe to re-run.
-- (Filtered indexes need QUOTED_IDENTIFIER ON; sqlcmd defaults to OFF.)
SET QUOTED_IDENTIFIER ON;
GO

IF COL_LENGTH('ProviderHospitalAgreements', 'AgreementNo') IS NULL
    ALTER TABLE ProviderHospitalAgreements ADD
        AgreementNo          NVARCHAR(100)  NULL,   -- e.g. DGMH/Mech.Laundry/SBDSNPR/132
        ScheduleNo           INT            NULL,   -- tender Schedule 1-12
        LoiNo                NVARCHAR(100)  NULL,   -- Letter of Intent number
        LoiDate              DATE           NULL,
        ContractValueInclGST DECIMAL(18,2)  NULL,   -- yearly value as written in the agreement (incl. 18% GST)
        BgNo                 NVARCHAR(60)   NULL,   -- Performance Security bank guarantee
        BgAmount             DECIMAL(18,2)  NULL,
        BgDate               DATE           NULL,   -- validity: 1 year 6 months from the agreement
        BgBank               NVARCHAR(150)  NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Agreements_AgreementNo' AND object_id = OBJECT_ID('ProviderHospitalAgreements'))
    CREATE UNIQUE INDEX UX_Agreements_AgreementNo
        ON ProviderHospitalAgreements (AgreementNo) WHERE AgreementNo IS NOT NULL;
GO
