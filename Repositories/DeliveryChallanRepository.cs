using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class DeliveryChallanRepository : IDeliveryChallanRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _db;

        public DeliveryChallanRepository(IConfiguration config, IDbConnection db)
        {
            _config = config;
            _db = db;

        }
        public async Task<DeliveryChallanVM> GetPickupForDelivery(int pickupId)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            using var multi =
                await con.QueryMultipleAsync(
                    "sp_GetPickupForDelivery",
                    new
                    {
                        PickupId = pickupId
                    },
                    commandType: CommandType.StoredProcedure);

            var model =
                await multi.ReadFirstOrDefaultAsync<DeliveryChallanVM>();

            if (model != null)
            {
                model.Items =
                    (await multi.ReadAsync<DeliveryChallanItemVM>())
                    .ToList();
            }

            return model;
        }

        public async Task<int> SaveDelivery(DeliveryChallanVM model)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var dt = new DataTable();

            dt.Columns.Add("LinenTypeId", typeof(int));
            dt.Columns.Add("DeliveryQty", typeof(int));

            foreach (var item in model.Items)
            {
                dt.Rows.Add(
                    item.LinenTypeId,
                    item.DeliveryQty);
            }

            DynamicParameters param = new();

            param.Add("@PickupId", model.PickupId);
            param.Add("@DeliveredBy", model.DeliveredBy);
            param.Add("@ReceivedBy", model.ReceivedBy);
            param.Add("@Remarks", model.Remarks);

            param.Add(
                "@Items",
                dt.AsTableValuedParameter(
                    "dbo.DeliveryItemType"));

            return await con.ExecuteScalarAsync<int>(
                "sp_SaveDelivery",
                param,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<List<DeliveryListVM>> GetDeliveryList()
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<DeliveryListVM>(
                    "sp_GetDeliveryList",
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<List<DeliveryChallanItemVM>>
        GetDeliveryItems(int deliveryId)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<DeliveryChallanItemVM>(
                    "sp_GetDeliveryItems",
                    new
                    {
                        DeliveryId = deliveryId
                    },
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
    }
}