using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IAgreementRepository
    {
        Task InsertAsync(AgreementVM model, string? filePath);
        Task<IEnumerable<AgreementVM>> GetAllAsync();
        Task<AgreementVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task SaveAsync(AgreementVM model, string? filePath);
    }
}
