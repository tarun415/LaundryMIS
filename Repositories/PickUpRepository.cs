using Dapper;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class PickUpRepository : IPickUpRepository
    {
        private readonly IConfiguration _config;

        public PickUpRepository(IConfiguration config)
        {
            _config = config;
        }

        // =========================
        // SAVE PICKUP
        // =========================
        public async Task<int> SavePickup(PickupVM model)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var dt = new DataTable();

            dt.Columns.Add("LinenTypeId", typeof(int));
            dt.Columns.Add("CollectedQty", typeof(int));

            foreach (var item in model.Items)
            {
                dt.Rows.Add(
                    item.LinenTypeId,
                    item.CollectedQty);
            }

            var param = new DynamicParameters();

            param.Add("@PickupId", model.PickupId);

            param.Add("@HospitalId", model.HospitalId);

            param.Add("@ProviderId", model.ProviderId);

            param.Add("@WardId", model.WardId);

            param.Add("@AgreementId", model.AgreementId);

            param.Add("@PickupDateTime", model.PickupDateTime);

            param.Add("@ShiftName", model.ShiftName);

            param.Add("@PickupBy", model.PickupBy);

            param.Add("@ReceivedBy", model.ReceivedBy);

            param.Add("@Remarks", model.Remarks);

            param.Add("@CreatedBy", model.CreatedBy);

            param.Add("@IsInfected", model.IsInfected);

            param.Add(
                "@PickupItems",
                dt.AsTableValuedParameter("dbo.PickupItemType"));

            var result =
                await con.QueryFirstOrDefaultAsync<DbResponse>(
                    "sp_SaveLaundryPickup",
                    param,
                    commandType: CommandType.StoredProcedure);

            if (result == null || result.Flag == 0)
            {
                throw new Exception(
                    result?.Message ?? "Pickup save failed");
            }

            return result.PickupId;
        }

        // =========================
        // PICKUP LIST
        // =========================
        public async Task<List<PickupListVM>> GetPickupList()
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<PickupListVM>(
                    "sp_GetPickupList",
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        // =========================
        // PICKUP ITEM DETAILS
        // =========================
        public async Task<List<PickupItemListVM>> GetPickupItems(
            int pickupId)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<PickupItemListVM>(
                    "sp_GetPickupItems",
                    new
                    {
                        PickupId = pickupId
                    },
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        // =========================
        // DELETE PICKUP
        // =========================
        public async Task<DbResponse> DeletePickup(int pickupId)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryFirstOrDefaultAsync<DbResponse>(
                    "sp_DeletePickup",
                    new
                    {
                        PickupId = pickupId
                    },
                    commandType: CommandType.StoredProcedure);

            return result;
        }

        // =========================
        // SEARCH PICKUP LIST
        // =========================
        public async Task<List<PickupListVM>> SearchPickupList(
            string status,
            int? hospitalId,
            int? wardId,
            DateTime? date)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<PickupListVM>(
                    "sp_SearchPickupList",
                    new
                    {
                        Status = status,
                        HospitalId = hospitalId,
                        WardId = wardId,
                        Date = date
                    },
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        // =========================
        // GET PICKUP BY ID
        // =========================
        public async Task<PickupVM> GetPickupById(int pickupId)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            using var multi =
                await con.QueryMultipleAsync(
                    "sp_GetPickupById",
                    new
                    {
                        PickupId = pickupId
                    },
                    commandType: CommandType.StoredProcedure);

            var master =
                await multi.ReadFirstOrDefaultAsync<PickupVM>();

            if (master != null)
            {
                master.Items =
                    (await multi.ReadAsync<PickupItemVM>())
                    .ToList();
            }

            return master;
        }

        public async Task UpdatePrintUrl(
    int pickupId,
    string path)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            await con.ExecuteAsync(
                @"UPDATE LaundryPickup
          SET PrintUrlFilePath = @path
          WHERE PickupId = @pickupId",
                new
                {
                    pickupId,
                    path
                });
        }
    }
}