-- PaymentApprovalLog ke asli columns: ApprovalLevel, ActionTaken, ActionOn
-- (purane procs Status / ActionDate likh rahe the jo table mein nahi hain).
-- Ab sirf 'Pending' payment pe action hota hai, aur affected rows return hote hain.

CREATE OR ALTER PROC dbo.sp_ApprovePayment
(
      @PaymentId  INT,
      @ApprovedBy INT,
      @Remarks    NVARCHAR(1000)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        UPDATE PaymentMaster
        SET Status     = 'Approved',
            ApprovedBy = @ApprovedBy,
            ApprovedOn = GETDATE(),
            ModifiedBy = @ApprovedBy,
            ModifiedOn = GETDATE()
        WHERE PaymentId = @PaymentId
          AND Status    = 'Pending';

        DECLARE @Rows INT = @@ROWCOUNT;

        IF @Rows > 0
            INSERT INTO PaymentApprovalLog
                (PaymentId, ApprovalLevel, ActionTaken, Remarks, ActionBy, ActionOn)
            VALUES
                (@PaymentId, 1, 'Approved', @Remarks, @ApprovedBy, GETDATE());

        COMMIT TRAN;

        SELECT @Rows;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROC dbo.sp_RejectPayment
(
      @PaymentId  INT,
      @RejectedBy INT,
      @Remarks    NVARCHAR(1000)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        UPDATE PaymentMaster
        SET Status     = 'Rejected',
            Remarks    = @Remarks,
            ModifiedBy = @RejectedBy,
            ModifiedOn = GETDATE()
        WHERE PaymentId = @PaymentId
          AND Status    = 'Pending';

        DECLARE @Rows INT = @@ROWCOUNT;

        IF @Rows > 0
            INSERT INTO PaymentApprovalLog
                (PaymentId, ApprovalLevel, ActionTaken, Remarks, ActionBy, ActionOn)
            VALUES
                (@PaymentId, 1, 'Rejected', @Remarks, @RejectedBy, GETDATE());

        COMMIT TRAN;

        SELECT @Rows;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
GO
