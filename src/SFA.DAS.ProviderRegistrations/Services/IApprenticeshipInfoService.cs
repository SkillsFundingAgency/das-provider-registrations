using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface IApprenticeshipInfoService
    {
        Provider GetProvider(long ukprn);
    }
}
