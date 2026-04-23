using LaudaryMis.Models;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAll();
    }
}
