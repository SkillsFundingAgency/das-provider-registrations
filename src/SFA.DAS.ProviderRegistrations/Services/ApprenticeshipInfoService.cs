using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Infastructure;
using SFA.Roatp.Api.Client;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public class ApprenticeshipInfoService : IApprenticeshipInfoService
    {
        private readonly ProviderRegistrationsSettings _configuration;

        public ApprenticeshipInfoService(ProviderRegistrationsSettings configuration)
        {
            _configuration = configuration;
        }

        public Roatp.Api.Types.Provider GetProvider(long ukprn)
        {
            var api = new RoatpApiClient(_configuration.RoatpApiClientBaseUrl);
            return api.Get(ukprn);
        }
    }
}
