using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IDisputeService
    {
        Task<(bool Success, string Message)> RaiseAsync(int providerId, int userId, RaiseDisputeVM model);

        Task<List<DisputeListItemVM>> GetListAsync(int? providerId, int? hospitalId, string? status);
        Task<DisputeListItemVM?> GetAsync(int id);
        Task<int> CountOpenAsync(int? hospitalId);

        // The admin's fix-the-WPR page
        Task<DisputeResolveVM?> GetResolveViewAsync(int id);

        // Admin changes the WPR scores, uploads the CMS letter and closes the dispute
        Task<(bool Success, string Message)> ResolveAsync(int adminId, DisputeResolvePost post);
        Task<(bool Success, string Message)> RejectAsync(int adminId, int id, string? remarks);

        // Full path of the stored CMS letter, or null
        string? LetterPath(DisputeListItemVM dispute);
    }
}
