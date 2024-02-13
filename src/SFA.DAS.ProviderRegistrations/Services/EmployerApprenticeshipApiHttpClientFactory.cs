using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Services;

public class EmployerApprenticeshipApiHttpClientFactory : IEmployerUsersApiHttpClientFactory
{
    private readonly EmployerApprenticeshipApiClientSettings _employerApprenticeshipApiClientSettings;

    public EmployerApprenticeshipApiHttpClientFactory(EmployerApprenticeshipApiClientSettings employerApprenticeshipApiClientSettings)
    {
        _employerApprenticeshipApiClientSettings = employerApprenticeshipApiClientSettings;
    }

    public IRestHttpClient CreateRestHttpClient()
    {
        return new RestHttpClient(new ManagedIdentityHttpClientFactory(_employerApprenticeshipApiClientSettings).CreateHttpClient());
    }
}