CREATE TYPE dbo.PickupItemType AS TABLE
(
    LinenTypeId INT,
    CollectedQty INT
)
GO
CREATE OR ALTER PROCEDURE sp_SaveLaundryPickup
(
    @AgreementId INT,
    @HospitalId INT,
    @ProviderId INT,
    @WardId INT,
    @ShiftName NVARCHAR(50),
    @PickupBy NVARCHAR(100),
    @ReceivedBy NVARCHAR(100),
    @Remarks NVARCHAR(500),
    @CreatedBy INT,

    @PickupItems dbo.PickupItemType READONLY
)
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        DECLARE @PickupId INT;
        DECLARE @PickupNo NVARCHAR(50);

        SET @PickupNo =
        'PICK-' +
        FORMAT(GETDATE(),'yyyyMMdd') +
        '-' +
        CAST(ABS(CHECKSUM(NEWID())) % 9999 AS NVARCHAR);

        INSERT INTO LaundryPickup
        (
            PickupNo,
            AgreementId,
            HospitalId,
            ProviderId,
            WardId,
            PickupDateTime,
            ShiftName,
            PickupBy,
            ReceivedBy,
            Status,
            Remarks,
            CreatedBy
        )
        VALUES
        (
            @PickupNo,
            @AgreementId,
            @HospitalId,
            @ProviderId,
            @WardId,
            GETDATE(),
            @ShiftName,
            @PickupBy,
            @ReceivedBy,
            'Pending Acceptance',
            @Remarks,
            @CreatedBy
        );

        SET @PickupId = SCOPE_IDENTITY();

        INSERT INTO LaundryPickupItems
        (
            PickupId,
            LinenTypeId,
            CollectedQty
        )
        SELECT
            @PickupId,
            LinenTypeId,
            CollectedQty
        FROM @PickupItems;

        UPDATE LaundryPickup
        SET TotalCollectedQty =
        (
            SELECT SUM(CollectedQty)
            FROM LaundryPickupItems
            WHERE PickupId = @PickupId
        )
        WHERE PickupId = @PickupId;

        COMMIT TRANSACTION;

        SELECT 1 AS Flag,
               'Pickup Saved Successfully' AS Message,
               @PickupId AS PickupId;

    END TRY

    BEGIN CATCH

        ROLLBACK TRANSACTION;

        SELECT 0 AS Flag,
               ERROR_MESSAGE() AS Message,
               0 AS PickupId;

    END CATCH

END
GO