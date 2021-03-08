using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public class RoatpApiHttpClientFactory : IRoatpApiHttpClientFactory
    {
        private readonly RoatpApiClientSettings _settings;

        public RoatpApiHttpClientFactory (RoatpApiClientSettings settings)
        {
            _settings = settings;
        }
        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new AzureActiveDirectoryHttpClientFactory(_settings).CreateHttpClient());
        }
    }
}