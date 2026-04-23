using LaudaryMis.Models;

namespace LaudaryMis.Services.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<Provider>> GetAll();
    }
}
