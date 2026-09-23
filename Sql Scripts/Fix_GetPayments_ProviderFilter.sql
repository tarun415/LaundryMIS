-- sp_GetPayments: @ProviderId filter add kiya taaki Provider sirf apni payments dekhe.
-- Status/Warning columns bhi return kiye (list aur warning letter button ke liye).

CREATE OR ALTER PROCEDURE dbo.sp_GetPayments
(
      @AgreementId INT = NULL,
      @HospitalId  INT = NULL,
      @ProviderId  INT = NULL,
      @MonthNo     INT = NULL,
      @YearNo      INT = NULL,
      @Status      VARCHAR(30) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        PM.PaymentId,
        PM.PaymentNo,

        PM.AgreementId,
        PM.ProviderId,
        PM.HospitalId,

        H.HospitalName,
        P.ProviderName,

        PM.MonthNo,
        PM.YearNo,

        PM.SanctionedBeds,
        PM.BedOccupancy,
        PM.RatePerBed,
        PM.MonthlyBill,

        PM.AverageScore,
        PM.PaymentPercentage,

        PM.GrossPayable,
        PM.GSTPercentage,
        PM.GSTAmount,
        PM.InvoiceAmount,
        PM.TDSPercentage,
        PM.TDSAmount,
        PM.NetPayable,

        PM.Status,
        PM.Remarks,

        PM.CreatedOn,
        PM.CreatedBy,
        PM.ApprovedOn,
        PM.ApprovedBy,

        PM.PaymentDate,
        PM.PaymentReferenceNo,

        PM.InvoiceGenerated,
        PM.InvoiceId,

        PM.WarningGenerated,
        PM.LatestWarningId AS WarningId

    FROM PaymentMaster PM
    INNER JOIN ProviderHospitalAgreements A ON PM.AgreementId = A.Id
    INNER JOIN tbl_Providers P              ON PM.ProviderId  = P.ProviderId
    INNER JOIN tbl_Hospitals H              ON PM.HospitalId  = H.HospitalId

    WHERE (@AgreementId IS NULL OR PM.AgreementId = @AgreementId)
      AND (@HospitalId  IS NULL OR PM.HospitalId  = @HospitalId)
      AND (@ProviderId  IS NULL OR PM.ProviderId  = @ProviderId)
      AND (@MonthNo     IS NULL OR PM.MonthNo     = @MonthNo)
      AND (@YearNo      IS NULL OR PM.YearNo      = @YearNo)
      AND (@Status      IS NULL OR PM.Status      = @Status)

    ORDER BY PM.CreatedOn DESC;
END
GO
