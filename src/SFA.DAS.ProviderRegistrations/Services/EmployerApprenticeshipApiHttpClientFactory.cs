using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Services
{
    class EmployerApprenticeshipApiHttpClientFactory : IEmployerUsersApiHttpClientFactory
    {
        private readonly EmployerApprenticeshipApiClientSettings _employerApprenticeshipApiClientSettings;

        public EmployerApprenticeshipApiHttpClientFactory(EmployerApprenticeshipApiClientSettings employerApprenticeshipApiClientSettings)
        {
            _employerApprenticeshipApiClientSettings = employerApprenticeshipApiClientSettings;
        }

        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new AzureActiveDirectoryHttpClientFactory(_employerApprenticeshipApiClientSettings).CreateHttpClient());
        }
    }
}
