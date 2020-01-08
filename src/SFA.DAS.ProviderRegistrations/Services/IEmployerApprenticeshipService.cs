using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface IEmployerApprenticeshipService
    {
        Task<bool> IsEmailAddressInUse(string emailAddress, CancellationToken cancellationToken = default);
    }
}
