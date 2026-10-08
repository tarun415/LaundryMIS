using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDisputeRepository
    {
        // False when this hospital + month already has an open dispute
        Task<bool> InsertAsync(int providerId, int hospitalId, int month, int year, string remarks, int userId);

        Task<List<DisputeListItemVM>> GetListAsync(int? providerId, int? hospitalId, string? status);
        Task<DisputeListItemVM?> GetByIdAsync(int id);
        Task<int> CountOpenAsync(int? hospitalId);

        // Both only touch a dispute that is still open; false when it no longer is
        Task<bool> ResolveAsync(int id, int adminId, string remarks, string? letterFile, string? letterOriginalName);
        Task<bool> RejectAsync(int id, int adminId, string remarks);
    }
}
