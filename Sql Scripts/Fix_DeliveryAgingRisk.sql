/*
    Fix: Delivery Aging Report risk level (tester feedback rows 17 & 76)

    Problem
    -------
    A pickup that is 100% returned (Linen Pending Return = 0) was still being
    shown with Risk Level = CRITICAL / WARNING, purely because the collection
    date was old (i.e. the delivery turnaround took more than 3/7 days).

    Expected (per QA)
    -----------------
    If Linen Pending Return = 0 and Return Completion = 100%, the pickup has no
    outstanding linen, so the Risk Level must be Normal regardless of how long
    the delivery took or how old the collection date is.

    Change
    ------
    In both aging stored procedures, any row with PendingQty <= 0 (fully
    returned) is classified as 'Normal'. Only rows that still have pending
    linen are aged into Warning / Critical by the number of days pending.

    NOTE: The application (ReportService.GetDeliveryAgingReport) also applies
    the same guard in C# as defense-in-depth, so the report is already correct
    even before this script is run. Running this script fixes the risk at the
    data source as well, so any other consumer of these procedures is correct too.

    Idempotent: safe to run multiple times (CREATE OR ALTER).
*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROC [dbo].[sp_GetDeliveryAgingReport]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        LP.PickupId,
        LP.PickupNo,
        H.HospitalName,
        W.WardName,
        LP.PickupDateTime AS PickupDate,
        ISNULL(LP.TotalCollectedQty, 0) AS CollectedQty,
        ISNULL(D.DeliveredQty, 0) AS DeliveredQty,

        -- Pending Quantity (never negative)
        CASE
            WHEN ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0) < 0
                THEN 0
            ELSE ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0)
        END AS PendingQty,

        LP.PickupDateTime,
        D.FinalDeliveryDate,

        -- Aging Days
        CASE
            -- Fully delivered: turnaround = final delivery date - pickup date
            WHEN ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0) <= 0
                THEN DATEDIFF(DAY, LP.PickupDateTime, D.FinalDeliveryDate)
            -- Still pending: today - pickup date
            ELSE DATEDIFF(DAY, LP.PickupDateTime, GETDATE())
        END AS AgingDays,

        -- Risk Level
        CASE
            -- Fully returned (no outstanding linen) => always Normal,
            -- regardless of how long the delivery took.
            WHEN ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0) <= 0
                THEN 'Normal'

            -- Still pending > 7 days
            WHEN DATEDIFF(DAY, LP.PickupDateTime, GETDATE()) > 7
                THEN 'Critical'

            -- Still pending 3-7 days
            WHEN DATEDIFF(DAY, LP.PickupDateTime, GETDATE()) BETWEEN 3 AND 7
                THEN 'Warning'

            -- Still pending 0-2 days
            ELSE 'Normal'
        END AS AgingStatus

    FROM LaundryPickup LP
    INNER JOIN tbl_Hospitals H
        ON H.HospitalId = LP.HospitalId
    INNER JOIN tbl_Wards W
        ON W.WardId = LP.WardId
    OUTER APPLY
    (
        SELECT
            SUM(ISNULL(DCI.DeliveredQty, 0)) AS DeliveredQty,
            MAX(DC.DeliveryDateTime) AS FinalDeliveryDate
        FROM DeliveryChallanItems DCI
        INNER JOIN DeliveryChallan DC
            ON DC.DeliveryId = DCI.DeliveryId
        WHERE DC.PickupId = LP.PickupId
    ) D
    WHERE ISNULL(LP.Status, '') <> 'Verified'
    ORDER BY AgingDays DESC;
END
GO

CREATE OR ALTER PROC [dbo].[sp_GetDeliveryAgingSummary]
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH AgingData AS
    (
        SELECT
            LP.PickupId,
            ISNULL(D.DeliveredQty, 0) AS DeliveredQty,

            -- Pending Quantity (never negative)
            CASE
                WHEN ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0) < 0
                    THEN 0
                ELSE ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0)
            END AS PendingQty,

            -- Aging Days
            CASE
                WHEN ISNULL(LP.TotalCollectedQty, 0) - ISNULL(D.DeliveredQty, 0) <= 0
                    THEN DATEDIFF(DAY, LP.PickupDateTime, D.FinalDeliveryDate)
                ELSE DATEDIFF(DAY, LP.PickupDateTime, GETDATE())
            END AS AgingDays
        FROM LaundryPickup LP
        OUTER APPLY
        (
            SELECT
                SUM(ISNULL(DCI.DeliveredQty, 0)) AS DeliveredQty,
                MAX(DC.DeliveryDateTime) AS FinalDeliveryDate
            FROM DeliveryChallanItems DCI
            INNER JOIN DeliveryChallan DC
                ON DC.DeliveryId = DCI.DeliveryId
            WHERE DC.PickupId = LP.PickupId
        ) D
        WHERE ISNULL(LP.Status, '') <> 'Verified'
    ),
    FinalAgingData AS
    (
        SELECT
            PickupId,
            PendingQty,
            AgingDays,
            CASE
                -- Fully returned => always Normal
                WHEN PendingQty <= 0
                    THEN 'Normal'
                -- Pending > 7 days
                WHEN PendingQty > 0 AND AgingDays > 7
                    THEN 'Critical'
                -- Pending 3-7 days
                WHEN PendingQty > 0 AND AgingDays BETWEEN 3 AND 7
                    THEN 'Warning'
                -- Pending 0-2 days
                ELSE 'Normal'
            END AS AgingStatus
        FROM AgingData
    )
    SELECT
        COUNT(CASE WHEN PendingQty > 0 THEN 1 END) AS TotalPendingPickups,
        ISNULL(SUM(CASE WHEN PendingQty > 0 THEN PendingQty ELSE 0 END), 0) AS TotalPendingQty,
        SUM(CASE WHEN AgingStatus = 'Normal'   THEN 1 ELSE 0 END) AS NormalCount,
        SUM(CASE WHEN AgingStatus = 'Warning'  THEN 1 ELSE 0 END) AS WarningCount,
        SUM(CASE WHEN AgingStatus = 'Critical' THEN 1 ELSE 0 END) AS CriticalCount
    FROM FinalAgingData;
END
GO
