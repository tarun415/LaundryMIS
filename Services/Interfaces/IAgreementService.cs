using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IAgreementService
    {
        Task SaveAsync(AgreementVM model, string? filePath);
        Task<AgreementVM> GetAgreementByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AgreementVM>> GetAllAsync();

    }
}
