/*
    Feature: Show handover people on delivery records (tester feedback row 78)

    Requirement
    -----------
    The delivery record / list should show:
      - Delivered By : name + phone number
      - Received By  : name + phone number
      - Provider     : name + registered phone number

    Findings in existing schema
    ---------------------------
    * DeliveryChallan stored ReceivedBy, but the "Delivered By" name entered by
      the provider was DISCARDED (sp_SaveDelivery inserted NULL into
      DeliveredByUserId). No phone numbers were captured at all.
    * tbl_Providers had no Phone column (provider phone was never stored,
      even though the Provider form model already had a Phone property).

    This script (all additive / idempotent):
      1. Adds DeliveredBy, DeliveredByPhone, ReceivedByPhone to DeliveryChallan.
      2. Adds Phone to tbl_Providers.
      3. Rewrites sp_SaveDelivery to persist the delivered-by name and both phones.
      4. Rewrites sp_GetDeliveryList to return the handover names, phones and the
         provider name + phone (joined via LaundryPickup.ProviderId).

    Run this once against the LaundaryMisLive database.
*/

------------------------------------------------------------
-- 1. DeliveryChallan: handover name + phone columns
------------------------------------------------------------
IF COL_LENGTH('dbo.DeliveryChallan', 'DeliveredBy') IS NULL
    ALTER TABLE dbo.DeliveryChallan ADD DeliveredBy NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.DeliveryChallan', 'DeliveredByPhone') IS NULL
    ALTER TABLE dbo.DeliveryChallan ADD DeliveredByPhone NVARCHAR(20) NULL;
GO
IF COL_LENGTH('dbo.DeliveryChallan', 'ReceivedByPhone') IS NULL
    ALTER TABLE dbo.DeliveryChallan ADD ReceivedByPhone NVARCHAR(20) NULL;
GO

------------------------------------------------------------
-- 2. tbl_Providers: registered phone number
------------------------------------------------------------
IF COL_LENGTH('dbo.tbl_Providers', 'Phone') IS NULL
    ALTER TABLE dbo.tbl_Providers ADD Phone NVARCHAR(20) NULL;
GO

------------------------------------------------------------
-- 3. sp_SaveDelivery : persist delivered-by name + phones
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_SaveDelivery]
(
      @PickupId         INT
    , @DeliveredBy      NVARCHAR(200)
    , @ReceivedBy       NVARCHAR(200)
    , @Remarks          NVARCHAR(500)
    , @DeliveredByPhone NVARCHAR(20)  = NULL
    , @ReceivedByPhone  NVARCHAR(20)  = NULL
    , @Items            DeliveryItemType READONLY
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- CHECK PICKUP EXISTS
        IF NOT EXISTS (SELECT 1 FROM LaundryPickup WHERE PickupId = @PickupId)
        BEGIN
            RAISERROR('Invalid Pickup.', 16, 1);
            RETURN;
        END

        -- GENERATE DELIVERY NO
        DECLARE @DeliveryNo NVARCHAR(100);
        SET @DeliveryNo =
            'DEL-'
            + CONVERT(VARCHAR(8), GETDATE(), 112)
            + '-'
            + RIGHT('0000' + CAST(
                ISNULL((SELECT COUNT(*) FROM DeliveryChallan
                        WHERE CAST(CreatedOn AS DATE) = CAST(GETDATE() AS DATE)), 0) + 1
                AS VARCHAR), 4);

        -- INSERT DELIVERY CHALLAN
        INSERT INTO DeliveryChallan
        (
              DeliveryNo
            , PickupId
            , DeliveredByUserId
            , DeliveredBy
            , DeliveredByPhone
            , ReceivedBy
            , ReceivedByPhone
            , DeliveryDateTime
            , Remarks
            , Status
            , IsPartialDelivery
            , CreatedOn
        )
        VALUES
        (
              @DeliveryNo
            , @PickupId
            , NULL
            , @DeliveredBy
            , @DeliveredByPhone
            , @ReceivedBy
            , @ReceivedByPhone
            , GETDATE()
            , @Remarks
            , 'Delivered'
            , 0
            , GETDATE()
        );

        DECLARE @DeliveryId INT = SCOPE_IDENTITY();

        -- DELIVERY ITEMS
        INSERT INTO DeliveryChallanItems (DeliveryId, LinenTypeId, DeliveredQty)
        SELECT @DeliveryId, LinenTypeId, DeliveryQty
        FROM @Items;

        -- UPDATE PICKUP ITEMS
        UPDATE P
        SET P.DeliveredQty = ISNULL(P.DeliveredQty, 0) + I.DeliveryQty
        FROM LaundryPickupItems P
        INNER JOIN @Items I ON P.LinenTypeId = I.LinenTypeId
        WHERE P.PickupId = @PickupId;

        -- CHECK PENDING
        DECLARE @PendingCount INT;
        SELECT @PendingCount = COUNT(*)
        FROM LaundryPickupItems
        WHERE PickupId = @PickupId
          AND (CollectedQty - ISNULL(DeliveredQty, 0)) > 0;

        -- UPDATE STATUS
        IF @PendingCount > 0
        BEGIN
            UPDATE DeliveryChallan
            SET IsPartialDelivery = 1, Status = 'Partial Delivered'
            WHERE DeliveryId = @DeliveryId;

            UPDATE LaundryPickup
            SET Status = 'Partial Delivered'
            WHERE PickupId = @PickupId;
        END
        ELSE
        BEGIN
            UPDATE DeliveryChallan
            SET IsPartialDelivery = 0, Status = 'Delivered'
            WHERE DeliveryId = @DeliveryId;

            UPDATE LaundryPickup
            SET Status = 'Delivered'
            WHERE PickupId = @PickupId;
        END

        COMMIT TRANSACTION;

        SELECT
              1 AS Flag
            , 'Delivery saved successfully.' AS Message
            , @DeliveryId AS DeliveryId
            , @DeliveryNo AS DeliveryNo;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT 0 AS Flag, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

------------------------------------------------------------
-- 4. sp_GetDeliveryList : return handover + provider details
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_GetDeliveryList]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ROW_NUMBER() OVER (ORDER BY DC.DeliveryId DESC) AS RowNum,
        DC.DeliveryId,
        DC.DeliveryNo,
        LP.PickupNo,
        H.HospitalName,
        W.WardName,
        LP.TotalCollectedQty,
        SUM(DCI.DeliveredQty) AS DeliveredQty,
        LP.TotalCollectedQty - SUM(DCI.DeliveredQty) AS PendingQty,
        DC.Status,
        DC.DeliveryDateTime,
        DC.DeliveredBy,
        DC.DeliveredByPhone,
        DC.ReceivedBy,
        DC.ReceivedByPhone,
        PR.ProviderName,
        PR.Phone AS ProviderPhone
    FROM DeliveryChallan DC
    INNER JOIN LaundryPickup LP
        ON LP.PickupId = DC.PickupId
    INNER JOIN tbl_Hospitals H
        ON H.HospitalId = LP.HospitalId
    INNER JOIN tbl_Wards W
        ON W.WardId = LP.WardId
    INNER JOIN DeliveryChallanItems DCI
        ON DCI.DeliveryId = DC.DeliveryId
    LEFT JOIN tbl_Providers PR
        ON PR.ProviderId = LP.ProviderId
    GROUP BY
        DC.DeliveryId,
        DC.DeliveryNo,
        LP.PickupNo,
        H.HospitalName,
        W.WardName,
        LP.TotalCollectedQty,
        DC.Status,
        DC.DeliveryDateTime,
        DC.DeliveredBy,
        DC.DeliveredByPhone,
        DC.ReceivedBy,
        DC.ReceivedByPhone,
        PR.ProviderName,
        PR.Phone
    ORDER BY MAX(DC.PickupId) DESC, DC.DeliveryId DESC;
END
GO
