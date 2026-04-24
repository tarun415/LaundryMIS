using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IAgreementRepository
    {
        Task InsertAsync(AgreementVM model, string? filePath);
    }
}
