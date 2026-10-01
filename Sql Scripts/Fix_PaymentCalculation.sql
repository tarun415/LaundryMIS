-- Payment calculation contract (Part-III, Payment Mechanism) ke hisaab se:
--   Monthly bill   = operational beds × rate per bed PER YEAR ÷ 12   (pehle ÷12 nahi tha)
--   Gross payable  = monthly bill × WPR band %
--   GST (18%)      = gross payable pe alag se
--   TDS (2%)       = gross payable pe (GST ke bina)
--   Net payable    = gross payable + GST − TDS
-- WPR month column nvarchar hai aur number ("5") store karta hai, isliye string se match.

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

    -- Agreement rate (per bed per YEAR, GST ke bina)
    SELECT @RatePerBed = RatePerBed
    FROM ProviderHospitalAgreements
    WHERE Id = @AgreementId
      AND HospitalId = @HospitalId
      AND IsActive = 1;

    -- Monthly bill: saal ka rate ÷ 12
    SET @MonthlyBill = ROUND(@BedOccupancy * @RatePerBed / 12.0, 2);

    -- Mahine ka average WPR score
    SELECT @AverageScore = ISNULL(AVG(CAST(TotalScore AS DECIMAL(18,2))), 0)
    FROM WeeklyPerformanceReport
    WHERE AgreementId = @AgreementId
      AND HospitalId  = @HospitalId
      AND Month       = CAST(@MonthNo AS NVARCHAR(2))
      AND Year        = @YearNo;

    -- Payment band (MonthlyBillService.GetPaymentBandPercent jaisa hi)
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
