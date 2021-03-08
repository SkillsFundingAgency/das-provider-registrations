using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface IProviderService
    {
        Task<Models.Provider> GetProvider(long ukprn);
    }
}
