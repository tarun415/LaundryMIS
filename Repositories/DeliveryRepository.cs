using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly IDbConnection _db;

        public DeliveryRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<MonthlyVerificationListVM>>
       GetWeeklyVerificationAsync(
           int hospitalId,
           int month,
           int year)
        {
            var sql = @"SELECT

    ROW_NUMBER() OVER
    (
        ORDER BY
            YEAR(de.EntryDate) DESC,

            CASE
                WHEN DAY(de.EntryDate) BETWEEN 1 AND 7 THEN 1
                WHEN DAY(de.EntryDate) BETWEEN 8 AND 14 THEN 2
                WHEN DAY(de.EntryDate) BETWEEN 15 AND 21 THEN 3
                WHEN DAY(de.EntryDate) BETWEEN 22 AND 28 THEN 4
                ELSE 5
            END DESC
    ) AS RowNum,

    STRING_AGG
    (
        CAST(de.Id AS VARCHAR(20)),
        ','
    ) AS EntryIds,

    MAX(de.HospitalId) AS HospitalId,

    CASE
        WHEN DAY(de.EntryDate) BETWEEN 1 AND 7 THEN 1
        WHEN DAY(de.EntryDate) BETWEEN 8 AND 14 THEN 2
        WHEN DAY(de.EntryDate) BETWEEN 15 AND 21 THEN 3
        WHEN DAY(de.EntryDate) BETWEEN 22 AND 28 THEN 4
        ELSE 5
    END AS WeekNo,

    MIN(de.EntryDate) AS WeekStartDate,

    MAX(de.EntryDate) AS WeekEndDate,

    CASE

        WHEN COUNT(*) =
             SUM
             (
                 CASE
                     WHEN de.Status = 'Verified'
                     THEN 1
                     ELSE 0
                 END
             )
        THEN 'Verified'

        WHEN SUM
        (
            CASE
                WHEN de.Status IN ('Collected','Partial')
                THEN 1
                ELSE 0
            END
        ) > 0
        THEN 'Pending'

        ELSE 'Delivered'

    END AS Status,

    MAX(ho.HospitalName) AS HospitalName,

    MAX(wvl.Remark) AS Remark,

    MAX(wvllg.LogBookPath) AS LogBookPath,

    SUM(ISNULL(di.TotalPickupQty,0))
        AS TotalPickupQty,

    SUM(ISNULL(di.CleanDeliveredQty,0))
        AS CleanDeliveredQty,

    SUM(ISNULL(di.TotalPickupQty,0))
    -
    SUM(ISNULL(di.CleanDeliveredQty,0))
        AS TotalPendingQty

FROM DailyEntries de

LEFT JOIN Tbl_Hospitals ho
    ON de.HospitalId = ho.HospitalId

LEFT JOIN
(
    SELECT

        EntryId,

        SUM(ISNULL(DirtyCount,0))
            AS TotalPickupQty,

        SUM(ISNULL(CleanCount,0))
            AS CleanDeliveredQty

    FROM DailyEntryItems

    GROUP BY EntryId

) di
    ON de.Id = di.EntryId

LEFT JOIN WeeklyVerificationLog wvl
    ON wvl.WeekNo =
    (
        CASE
            WHEN DAY(de.EntryDate) BETWEEN 1 AND 7 THEN 1
            WHEN DAY(de.EntryDate) BETWEEN 8 AND 14 THEN 2
            WHEN DAY(de.EntryDate) BETWEEN 15 AND 21 THEN 3
            WHEN DAY(de.EntryDate) BETWEEN 22 AND 28 THEN 4
            ELSE 5
        END
    )
    AND wvl.HospitalId = de.HospitalId

LEFT JOIN WeeklyVerification wvllg
    ON wvllg.Id = wvl.WeeklyVerificationId
    AND MONTH(wvllg.FromDate) = MONTH(de.EntryDate)
    AND YEAR(wvllg.FromDate) = YEAR(de.EntryDate)

WHERE de.HospitalId = @hospitalId

AND MONTH(de.EntryDate) = @month

AND YEAR(de.EntryDate) = @year

GROUP BY

    YEAR(de.EntryDate),

    CASE
        WHEN DAY(de.EntryDate) BETWEEN 1 AND 7 THEN 1
        WHEN DAY(de.EntryDate) BETWEEN 8 AND 14 THEN 2
        WHEN DAY(de.EntryDate) BETWEEN 15 AND 21 THEN 3
        WHEN DAY(de.EntryDate) BETWEEN 22 AND 28 THEN 4
        ELSE 5
    END

ORDER BY

    YEAR(de.EntryDate) DESC,

    CASE
        WHEN DAY(de.EntryDate) BETWEEN 1 AND 7 THEN 1
        WHEN DAY(de.EntryDate) BETWEEN 8 AND 14 THEN 2
        WHEN DAY(de.EntryDate) BETWEEN 15 AND 21 THEN 3
        WHEN DAY(de.EntryDate) BETWEEN 22 AND 28 THEN 4
        ELSE 5
    END DESC";

            var data =
                await _db.QueryAsync<MonthlyVerificationListVM>(
                    sql,
                    new
                    {
                        hospitalId,
                        month,
                        year
                    });

            return data.ToList();
        }


        public async Task<List<MonthlyVerificationListVM>>
    GetWeeklyDrillDownAsync(
        int hospitalId,
        int month,
        int year,
        int weekNo)
        {
            var sql = @"
SELECT

    ROW_NUMBER() OVER
    (
        ORDER BY de.EntryDate DESC
    ) AS RowNum,

    de.Id AS EntryIds,

    de.EntryDate,

    de.Status AS Status,

    ho.HospitalName,

    ISNULL(di.TotalPickupQty,0)
        AS TotalPickupQty,

    ISNULL(di.CleanDeliveredQty,0)
        AS CleanDeliveredQty,

    ISNULL(di.TotalPickupQty,0)
    -
    ISNULL(di.CleanDeliveredQty,0)
        AS TotalPendingQty

FROM DailyEntries de

LEFT JOIN Tbl_Hospitals ho
    ON de.HospitalId = ho.HospitalId

LEFT JOIN
(
    SELECT

        EntryId,

        SUM(ISNULL(DirtyCount,0))
            AS TotalPickupQty,

        SUM(ISNULL(CleanCount,0))
            AS CleanDeliveredQty

    FROM DailyEntryItems

    GROUP BY EntryId

) di
    ON de.Id = di.EntryId

WHERE de.HospitalId = @hospitalId

AND MONTH(de.EntryDate) = @month

AND YEAR(de.EntryDate) = @year

AND
(
    (@weekNo = 1 AND DAY(de.EntryDate) BETWEEN 1 AND 7)

    OR

    (@weekNo = 2 AND DAY(de.EntryDate) BETWEEN 8 AND 14)

    OR

    (@weekNo = 3 AND DAY(de.EntryDate) BETWEEN 15 AND 21)

    OR

    (@weekNo = 4 AND DAY(de.EntryDate) BETWEEN 22 AND 28)

    OR

    (@weekNo = 5 AND DAY(de.EntryDate) BETWEEN 29
                                      AND DAY(EOMONTH(de.EntryDate)))
)

ORDER BY de.EntryDate DESC;


";

            var data =
                await _db.QueryAsync<MonthlyVerificationListVM>(
                    sql,
                    new
                    {
                        hospitalId,
                        month,
                        year,
                        weekNo
                    });

            return data.ToList();
        }


        public async Task<int> SaveWeeklyVerificationLogAsync(
      WeeklyVerificationModel model)
        {
            try
            {
                if (_db.State == ConnectionState.Closed)
                    _db.Open();

                using var tran = _db.BeginTransaction();

                try
                {
                    var insertSql = @"

INSERT INTO WeeklyVerificationLog
(
    HospitalId,
    TotalPickupQty,
    TotalDeliveredQty,
    TotalPendingQty,
    Status,
    WeekNo,
    FromDate,
    ToDate,
    Remark,
    CreatedBy,
    EntryIds
)
VALUES
(
    @HospitalId,
    @TotalPickupQty,
    @TotalDeliveredQty,
    @TotalPendingQty,
    @Status,
    @WeekNo,
    @FromDate,
    @ToDate,
    @Remark,
    @CreatedBy,
    @EntryIds
)

SELECT CAST(SCOPE_IDENTITY() AS INT)";

                    int result =
                        await _db.ExecuteScalarAsync<int>(
                            insertSql,
                            new
                            {
                                model.HospitalId,
                                model.TotalPickupQty,
                                model.TotalDeliveredQty,
                                model.TotalPendingQty,
                                model.Status,
                                model.WeekNo,
                                model.FromDate,
                                model.ToDate,
                                model.Remark,
                                model.CreatedBy,
                                model.EntryIds
                            },
                            tran);




                    // UPDATE DeliveryEntries

                    var updateDeliverySql = @"

UPDATE DeliveryEntries
SET IsVerified = 1
WHERE  EntryId in 
(
    SELECT value
    FROM STRING_SPLIT(@EntryIds, ',')
)";

                    await _db.ExecuteAsync(
                        updateDeliverySql,
                        new
                        {
                            EntryIds = model.EntryIds
                        },
                        tran);




                    // UPDATE DailyEntries STATUS

                    var updateDailySql = @"

UPDATE DailyEntries
SET Status = 'Verified'
WHERE Id IN
(
    SELECT value
    FROM STRING_SPLIT(@EntryIds, ',')
)";

                    await _db.ExecuteAsync(
                        updateDailySql,
                        new
                        {
                            EntryIds = model.EntryIds
                        },
                        tran);



                    tran.Commit();

                    return result;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> SaveMonthlyLogBookAsync(
    WeeklyVerificationModel model)
        {
            try
            {
                if (_db.State == ConnectionState.Closed)
                    _db.Open();

                using var tran = _db.BeginTransaction();

                try
                {
                    // Duplicate check

                    var existsSql = @"

SELECT COUNT(1)
FROM WeeklyVerification
WHERE MONTH(FromDate) = @Month
AND YEAR(FromDate) = @Year
AND Status = 'Verified'";

                    int exists = await _db.ExecuteScalarAsync<int>(
                        existsSql,
                        new
                        {
                            model.Month,
                            model.Year
                        },
                        tran);

                    if (exists > 0)
                    {
                        throw new Exception(
                            "Monthly logbook already uploaded.");
                    }

                    // INSERT INTO WeeklyVerification

                    var insertSql = @"

INSERT INTO WeeklyVerification
(
    Status,
    WeekNo,
    FromDate,
    ToDate,
    Remark,
    LogBookPath
)
VALUES
(
    'Verified',
    NULL,

    (
        SELECT MIN(FromDate)
        FROM WeeklyVerificationLog
        WHERE HospitalId = @HospitalId
        AND MONTH(FromDate) = @Month
        AND YEAR(FromDate) = @Year
    ),

    (
        SELECT MAX(ToDate)
        FROM WeeklyVerificationLog
        WHERE HospitalId = @HospitalId
        AND MONTH(ToDate) = @Month
        AND YEAR(ToDate) = @Year
    ),

    @Remark,
    @LogBookPath
)

SELECT CAST(SCOPE_IDENTITY() AS INT)";

                    int weeklyVerificationId =
                        await _db.ExecuteScalarAsync<int>(
                            insertSql,
                            new
                            {
                                model.HospitalId,
                                model.Month,
                                model.Year,
                                model.Remark,
                                model.LogBookPath
                            },
                            tran);

                    // UPDATE WeeklyVerificationLog

                    var updateSql = @"

UPDATE WeeklyVerificationLog
SET WeeklyVerificationId = @WeeklyVerificationId
WHERE HospitalId = @HospitalId
AND MONTH(FromDate) = @Month
AND YEAR(FromDate) = @Year";

                    await _db.ExecuteAsync(
                        updateSql,
                        new
                        {
                            WeeklyVerificationId = weeklyVerificationId,
                            model.HospitalId,
                            model.Month,
                            model.Year
                        },
                        tran);

                    tran.Commit();

                    return weeklyVerificationId;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        //        public async Task<int> SaveMonthlyLogBookAsync(
        //     WeeklyVerificationModel model)
        //        {
        //            try
        //            {
        //                if (_db.State == ConnectionState.Closed)
        //                    _db.Open();

        //                using var tran = _db.BeginTransaction();

        //                try
        //                {
        //                    // INSERT INTO WeeklyVerification

        //                    var insertSql = @"

        //INSERT INTO WeeklyVerification
        //(
        //    Status,
        //    WeekNo,
        //    FromDate,
        //    ToDate,
        //    Remark,
        //    LogBookPath
        //)
        //VALUES
        //(
        //    'Verified',
        //    0,
        //    (
        //        SELECT MIN(FromDate)
        //        FROM WeeklyVerificationLog
        //        WHERE HospitalId = @HospitalId
        //        AND MONTH(FromDate) = @Month
        //        AND YEAR(FromDate) = @Year
        //    ),
        //    (
        //        SELECT MAX(ToDate)
        //        FROM WeeklyVerificationLog
        //        WHERE HospitalId = @HospitalId
        //        AND MONTH(ToDate) = @Month
        //        AND YEAR(ToDate) = @Year
        //    ),
        //    @Remark,
        //    @LogBookPath
        //)

        //SELECT CAST(SCOPE_IDENTITY() AS INT)";

        //                    int weeklyVerificationId =
        //                        await _db.ExecuteScalarAsync<int>(
        //                            insertSql,
        //                            new
        //                            {
        //                                model.HospitalId,
        //                                model.Month,
        //                                model.Year,
        //                                model.Remark,
        //                                model.LogBookPath
        //                            },
        //                            tran);



        //                    // UPDATE WeeklyVerificationLog

        //                    var updateSql = @"

        //UPDATE WeeklyVerificationLog
        //SET WeeklyVerificationId = @WeeklyVerificationId
        //WHERE HospitalId = @HospitalId
        //AND MONTH(FromDate) = @Month
        //AND YEAR(FromDate) = @Year";

        //                    await _db.ExecuteAsync(
        //                        updateSql,
        //                        new
        //                        {
        //                            WeeklyVerificationId = weeklyVerificationId,
        //                            model.HospitalId,
        //                            model.Month,
        //                            model.Year
        //                        },
        //                        tran);


        //                    tran.Commit();

        //                    return weeklyVerificationId;
        //                }
        //                catch
        //                {
        //                    tran.Rollback();
        //                    throw;
        //                }
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //        }

    }
}