-- Vendor (provider) profile aur KYC documents — RFP Part I (Eligibility, Format 6)
-- Script dobara chalana safe hai.

IF OBJECT_ID('dbo.ProviderProfile', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProviderProfile
    (
        ProviderId               INT            NOT NULL PRIMARY KEY,

        -- Firm (Format 6, Part 2)
        LegalStatus              NVARCHAR(50)   NULL,   -- Company / LLP / Partnership / Proprietorship / Society / Trust
        RegistrationNo           NVARCHAR(100)  NULL,
        RegistrationAuthority    NVARCHAR(200)  NULL,
        GSTNo                    NVARCHAR(15)   NULL,
        PANNo                    NVARCHAR(10)   NULL,
        EPFNo                    NVARCHAR(50)   NULL,
        ESINo                    NVARCHAR(50)   NULL,
        MSMENo                   NVARCHAR(50)   NULL,
        Address                  NVARCHAR(500)  NULL,

        -- Contact person (Format 6, Part 1)
        ContactDesignation       NVARCHAR(100)  NULL,

        -- Contract Manager (RFP 2.3-V / Article 9)
        ContractManagerName      NVARCHAR(150)  NULL,
        ContractManagerPhone     NVARCHAR(15)   NULL,
        ContractManagerExpYears  INT            NULL,

        -- Payment ke liye bank
        BankAccountName          NVARCHAR(150)  NULL,
        BankAccountNo            NVARCHAR(30)   NULL,
        BankIFSC                 NVARCHAR(11)   NULL,
        BankName                 NVARCHAR(150)  NULL,
        BankBranch               NVARCHAR(150)  NULL,

        UpdatedOn                DATETIME       NOT NULL CONSTRAINT DF_ProviderProfile_UpdatedOn DEFAULT (GETDATE()),
        UpdatedBy                INT            NULL,

        CONSTRAINT FK_ProviderProfile_Provider
            FOREIGN KEY (ProviderId) REFERENCES dbo.tbl_Providers (ProviderId)
    );
END
GO

IF OBJECT_ID('dbo.ProviderDocuments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProviderDocuments
    (
        Id               INT IDENTITY(1,1) PRIMARY KEY,
        ProviderId       INT            NOT NULL,
        DocumentType     NVARCHAR(60)   NOT NULL,
        DocumentNo       NVARCHAR(100)  NULL,
        ValidTill        DATE           NULL,
        FileName         NVARCHAR(300)  NOT NULL,   -- disk pe naam (GUID)
        OriginalFileName NVARCHAR(300)  NULL,
        ContentType      NVARCHAR(100)  NULL,
        FileSize         BIGINT         NULL,
        UploadedOn       DATETIME       NOT NULL CONSTRAINT DF_ProviderDocuments_UploadedOn DEFAULT (GETDATE()),
        UploadedBy       INT            NULL,
        IsDeleted        BIT            NOT NULL CONSTRAINT DF_ProviderDocuments_IsDeleted DEFAULT (0),

        CONSTRAINT FK_ProviderDocuments_Provider
            FOREIGN KEY (ProviderId) REFERENCES dbo.tbl_Providers (ProviderId)
    );

    CREATE INDEX IX_ProviderDocuments_Provider ON dbo.ProviderDocuments (ProviderId, IsDeleted);
END
GO
