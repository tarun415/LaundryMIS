using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class ProviderProfileService : IProviderProfileService
    {
        private readonly IProviderProfileRepository _repo;

        public ProviderProfileService(IProviderProfileRepository repo)
        {
            _repo = repo;
        }

        // Profile ke saath uske documents bhi
        public async Task<ProviderProfileVM?> GetProfileAsync(int providerId)
        {
            var profile = await _repo.GetProfileAsync(providerId);
            if (profile != null)
                profile.Documents = await _repo.GetDocumentsAsync(providerId);
            return profile;
        }

        public async Task SaveProfileAsync(ProviderProfileVM model, int userId)
        {
            // Codes hamesha capital mein save hon
            model.GSTNo = Upper(model.GSTNo);
            model.PANNo = Upper(model.PANNo);
            model.BankIFSC = Upper(model.BankIFSC);

            await _repo.SaveProfileAsync(model, userId);
        }

        public Task<ProviderDocumentVM?> GetDocumentAsync(int documentId) =>
            _repo.GetDocumentAsync(documentId);

        public Task<int> AddDocumentAsync(ProviderDocumentVM document, int userId) =>
            _repo.AddDocumentAsync(document, userId);

        public Task<bool> DeleteDocumentAsync(int documentId, int providerId) =>
            _repo.DeleteDocumentAsync(documentId, providerId);

        private static string? Upper(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
    }
}
