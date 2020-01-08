using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Services
{
    class EmployerApprenticeshipApiHttpClientFactory : IEmployerUsersApiHttpClientFactory
    {
        private readonly EmployerApprenticeshipApiClientSettings _employerUsersApiClientSettings;

        public EmployerApprenticeshipApiHttpClientFactory(EmployerApprenticeshipApiClientSettings employerUsersApiClientSettings)
        {
            _employerUsersApiClientSettings = employerUsersApiClientSettings;
        }

        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new AzureActiveDirectoryHttpClientFactory(_employerUsersApiClientSettings).CreateHttpClient());
        }
    }
}
