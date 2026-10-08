-- CMS role + WPR verification + bill disputes.
-- Script dobara chalana safe hai: har cheez pehle check karti hai ki wo pehle se hai ya nahi.
--
-- Flow:
--   Hospital WPR submit karta hai (Status = 'Pending', phir hospital edit nahi kar sakta)
--   CMS (har hospital ka ek) WPR ki entries edit kar sakta hai aur verify karta hai ('Verified')
--   ServiceProvider ko sirf poore verified mahine ka bill print karne ko milta hai
--   ServiceProvider bill par dispute uthata hai, sirf Admin WPR badal kar (CMS ka letter upload karke) solve karta hai

-- ─────────────────────────────────────────────────────────
-- 1. Roles: 3 = ServiceProvider (pehle Provider), 4 = CMS
-- ─────────────────────────────────────────────────────────
UPDATE dbo.Tbl_Roles SET RoleName = 'ServiceProvider' WHERE RoleId = 3 AND RoleName = 'Provider';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Tbl_Roles WHERE RoleId = 4)
BEGIN
    IF COLUMNPROPERTY(OBJECT_ID('dbo.Tbl_Roles'), 'RoleId', 'IsIdentity') = 1
    BEGIN
        SET IDENTITY_INSERT dbo.Tbl_Roles ON;
        INSERT INTO dbo.Tbl_Roles (RoleId, RoleName) VALUES (4, 'CMS');
        SET IDENTITY_INSERT dbo.Tbl_Roles OFF;
    END
    ELSE
        INSERT INTO dbo.Tbl_Roles (RoleId, RoleName) VALUES (4, 'CMS');
END
GO

-- Admin ke banaye CMS password ko pehle login par badalna padta hai
IF COL_LENGTH('dbo.Tbl_Users', 'MustChangePassword') IS NULL
    ALTER TABLE dbo.Tbl_Users ADD MustChangePassword BIT NOT NULL
        CONSTRAINT DF_Tbl_Users_MustChangePassword DEFAULT (0);
GO

-- ─────────────────────────────────────────────────────────
-- 2. WPR status. Jo WPR pehle se hain wo 'Verified' maane jaate hain
--    (naye WPR 'Pending' se shuru hote hain).
-- ─────────────────────────────────────────────────────────
IF COL_LENGTH('dbo.WeeklyPerformanceReport', 'Status') IS NULL
BEGIN
    ALTER TABLE dbo.WeeklyPerformanceReport ADD Status NVARCHAR(20) NOT NULL
        CONSTRAINT DF_WPR_Status_Legacy DEFAULT ('Verified');

    EXEC('ALTER TABLE dbo.WeeklyPerformanceReport DROP CONSTRAINT DF_WPR_Status_Legacy');
    EXEC('ALTER TABLE dbo.WeeklyPerformanceReport ADD CONSTRAINT DF_WPR_Status DEFAULT (''Pending'') FOR Status');
END
GO

IF COL_LENGTH('dbo.WeeklyPerformanceReport', 'VerifiedBy') IS NULL
    ALTER TABLE dbo.WeeklyPerformanceReport ADD VerifiedBy INT NULL;
IF COL_LENGTH('dbo.WeeklyPerformanceReport', 'VerifiedAt') IS NULL
    ALTER TABLE dbo.WeeklyPerformanceReport ADD VerifiedAt DATETIME NULL;
IF COL_LENGTH('dbo.WeeklyPerformanceReport', 'LastEditedBy') IS NULL
    ALTER TABLE dbo.WeeklyPerformanceReport ADD LastEditedBy INT NULL;
IF COL_LENGTH('dbo.WeeklyPerformanceReport', 'LastEditedAt') IS NULL
    ALTER TABLE dbo.WeeklyPerformanceReport ADD LastEditedAt DATETIME NULL;
GO

-- Purane (pehle se verified maane gaye) WPR ka verify time submit time ke barabar
UPDATE dbo.WeeklyPerformanceReport
SET VerifiedAt = SubmittedAt
WHERE Status = 'Verified' AND VerifiedAt IS NULL;
GO

-- ─────────────────────────────────────────────────────────
-- 3. WPR edit log (CMS / Admin ne kya badla)
-- ─────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.WPREditLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.WPREditLog
    (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        WPRId         INT            NOT NULL,
        ParameterId   INT            NOT NULL,
        ParameterName NVARCHAR(200)  NULL,
        OldScore      INT            NULL,
        NewScore      INT            NOT NULL,
        OldTotal      INT            NULL,
        NewTotal      INT            NOT NULL,
        EditedBy      INT            NOT NULL,
        EditedByRole  NVARCHAR(20)   NOT NULL,
        EditedAt      DATETIME       NOT NULL CONSTRAINT DF_WPREditLog_EditedAt DEFAULT (GETDATE()),
        Remarks       NVARCHAR(500)  NULL,
        DisputeId     INT            NULL
    );

    CREATE INDEX IX_WPREditLog_WPRId ON dbo.WPREditLog (WPRId);
END
GO

-- ─────────────────────────────────────────────────────────
-- 4. Bill print log: kaun sa bill (kaun sa version) kab print hua
-- ─────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.BillPrintLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BillPrintLog
    (
        Id               INT IDENTITY(1,1) PRIMARY KEY,
        ProviderId       INT            NOT NULL,
        HospitalId       INT            NOT NULL,
        BillMonth        TINYINT        NOT NULL,
        BillYear         SMALLINT       NOT NULL,
        Version          INT            NOT NULL,
        WPRAvgScore      DECIMAL(6,2)   NOT NULL,
        NetPayableAmount DECIMAL(18,2)  NOT NULL,
        PrintedBy        INT            NOT NULL,
        PrintedAt        DATETIME       NOT NULL CONSTRAINT DF_BillPrintLog_PrintedAt DEFAULT (GETDATE())
    );

    CREATE INDEX IX_BillPrintLog_Month ON dbo.BillPrintLog (ProviderId, HospitalId, BillYear, BillMonth);
END
GO

-- ─────────────────────────────────────────────────────────
-- 5. Bill disputes
-- ─────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.BillDisputes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BillDisputes
    (
        Id                INT IDENTITY(1,1) PRIMARY KEY,
        ProviderId        INT             NOT NULL,
        HospitalId        INT             NOT NULL,
        BillMonth         TINYINT         NOT NULL,
        BillYear          SMALLINT        NOT NULL,
        Remarks           NVARCHAR(1000)  NOT NULL,
        Status            NVARCHAR(20)    NOT NULL CONSTRAINT DF_BillDisputes_Status DEFAULT ('Open'), -- Open / Resolved / Rejected
        RaisedBy          INT             NOT NULL,
        RaisedAt          DATETIME        NOT NULL CONSTRAINT DF_BillDisputes_RaisedAt DEFAULT (GETDATE()),
        ResolvedBy        INT             NULL,
        ResolvedAt        DATETIME        NULL,
        ResolutionRemarks NVARCHAR(1000)  NULL,
        LetterFile        NVARCHAR(300)   NULL,   -- CMS ka letter (resolve karte waqt)
        LetterOriginalName NVARCHAR(260)  NULL
    );

    CREATE INDEX IX_BillDisputes_Hospital ON dbo.BillDisputes (HospitalId, Status);
    CREATE INDEX IX_BillDisputes_Provider ON dbo.BillDisputes (ProviderId, Status);

    -- Ek hospital + mahine par ek hi dispute open reh sakta hai
    CREATE UNIQUE INDEX UX_BillDisputes_OneOpen
        ON dbo.BillDisputes (ProviderId, HospitalId, BillYear, BillMonth)
        WHERE Status = 'Open';
END
GO

-- ─────────────────────────────────────────────────────────
-- 6. Payment calculation sirf Verified WPR ka average leta hai
--    (baaki sab pehle jaisa: Fix_PaymentCalculation.sql)
-- ─────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE dbo.sp_GetPaymentCalculation
(
    @AgreementId  INT,
    @HospitalId   INT,
    @MonthNo      INT,
    @YearNo       INT,
    @BedOccupancy INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RatePerBed        DECIMAL(18,2) = 0;
    DECLARE @AverageScore      DECIMAL(18,2) = 0;
    DECLARE @PaymentPercentage DECIMAL(5,2)  = 0;

    DECLARE @MonthlyBill   DECIMAL(18,2) = 0;
    DECLARE @GrossPayable  DECIMAL(18,2) = 0;

    DECLARE @GSTPercentage DECIMAL(5,2)  = 18.00;
    DECLARE @GSTAmount     DECIMAL(18,2) = 0;
    DECLARE @InvoiceAmount DECIMAL(18,2) = 0;

    DECLARE @TDSPercentage DECIMAL(5,2)  = 2.00;
    DECLARE @TDSAmount     DECIMAL(18,2) = 0;
    DECLARE @NetPayable    DECIMAL(18,2) = 0;

    SELECT @RatePerBed = RatePerBed
    FROM ProviderHospitalAgreements
    WHERE Id = @AgreementId
      AND HospitalId = @HospitalId
      AND IsActive = 1;

    SET @MonthlyBill = ROUND(@BedOccupancy * @RatePerBed / 12.0, 2);

    -- Mahine ka average WPR score: sirf CMS-verified WPR
    SELECT @AverageScore = ISNULL(AVG(CAST(TotalScore AS DECIMAL(18,2))), 0)
    FROM WeeklyPerformanceReport
    WHERE AgreementId = @AgreementId
      AND HospitalId  = @HospitalId
      AND Month       = CAST(@MonthNo AS NVARCHAR(2))
      AND Year        = @YearNo
      AND Status      = 'Verified';

    SET @PaymentPercentage =
        CASE
            WHEN @AverageScore >= 81 THEN 100
            WHEN @AverageScore >= 71 THEN 90
            WHEN @AverageScore >= 61 THEN 80
            WHEN @AverageScore >= 41 THEN 60
            WHEN @AverageScore >= 21 THEN 40
            ELSE 0
        END;

    SET @GrossPayable  = ROUND(@MonthlyBill * @PaymentPercentage / 100, 2);
    SET @GSTAmount     = ROUND(@GrossPayable * @GSTPercentage / 100, 2);
    SET @InvoiceAmount = @GrossPayable + @GSTAmount;
    SET @TDSAmount     = ROUND(@GrossPayable * @TDSPercentage / 100, 2);
    SET @NetPayable    = @InvoiceAmount - @TDSAmount;

    SELECT
        @RatePerBed        AS RatePerBed,
        @BedOccupancy      AS BedOccupancy,
        @MonthlyBill       AS MonthlyBill,
        @AverageScore      AS AverageScore,
        @PaymentPercentage AS PaymentPercentage,
        @GrossPayable      AS GrossPayable,
        @GSTPercentage     AS GSTPercentage,
        @GSTAmount         AS GSTAmount,
        @InvoiceAmount     AS InvoiceAmount,
        @TDSPercentage     AS TDSPercentage,
        @TDSAmount         AS TDSAmount,
        @NetPayable        AS NetPayable;
END
GO
