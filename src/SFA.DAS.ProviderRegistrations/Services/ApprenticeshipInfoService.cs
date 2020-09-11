using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public class ApprenticeshipInfoService : IApprenticeshipInfoService
    {
        private readonly ProviderRegistrationsSettings _configuration;

        public ApprenticeshipInfoService(ProviderRegistrationsSettings configuration)
        {
            _configuration = configuration;
        }

        public Apprenticeships.Api.Types.Providers.Provider GetProvider(long ukprn)
        {
            var api = new ProviderApiClient(_configuration.ProviderApiClientBaseUrl);
            return api.Get(ukprn);
        }
    }
}
