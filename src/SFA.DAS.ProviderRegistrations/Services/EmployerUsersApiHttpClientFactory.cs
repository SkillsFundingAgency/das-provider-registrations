using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Services
{
    class EmployerUsersApiHttpClientFactory : IEmployerUsersApiHttpClientFactory
    {
        private readonly EmployerUsersApiClientSettings _employerUsersApiClientSettings;

        public EmployerUsersApiHttpClientFactory(EmployerUsersApiClientSettings employerUsersApiClientSettings)
        {
            _employerUsersApiClientSettings = employerUsersApiClientSettings;
        }

        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new AzureActiveDirectoryHttpClientFactory(_employerUsersApiClientSettings).CreateHttpClient());
        }
    }
}
