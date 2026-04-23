using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IAgreementService
    {
        Task SaveAsync(AgreementVM model, string filePath);

    }
}
