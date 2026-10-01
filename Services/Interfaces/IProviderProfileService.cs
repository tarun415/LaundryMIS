using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IProviderProfileService
    {
        Task<ProviderProfileVM?> GetProfileAsync(int providerId);
        Task SaveProfileAsync(ProviderProfileVM model, int userId);

        Task<ProviderDocumentVM?> GetDocumentAsync(int documentId);
        Task<int> AddDocumentAsync(ProviderDocumentVM document, int userId);
        Task<bool> DeleteDocumentAsync(int documentId, int providerId);
    }
}
