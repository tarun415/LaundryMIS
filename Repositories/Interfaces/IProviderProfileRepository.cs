using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IProviderProfileRepository
    {
        Task<ProviderProfileVM?> GetProfileAsync(int providerId);
        Task SaveProfileAsync(ProviderProfileVM model, int userId);

        Task<List<ProviderDocumentVM>> GetDocumentsAsync(int providerId);
        Task<ProviderDocumentVM?> GetDocumentAsync(int documentId);
        Task<ProviderDocumentVM?> GetLatestDocumentAsync(int providerId, string documentType);
        Task<int> AddDocumentAsync(ProviderDocumentVM document, int userId);
        Task<bool> DeleteDocumentAsync(int documentId, int providerId);
    }
}
