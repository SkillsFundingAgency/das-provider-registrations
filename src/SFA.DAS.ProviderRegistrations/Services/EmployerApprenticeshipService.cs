using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Services;

public class EmployerApprenticeshipService : IEmployerApprenticeshipService
{
    private readonly IRestHttpClient _httpClient;

    public EmployerApprenticeshipService(IEmployerUsersApiHttpClientFactory employerUsersApiHttpClientFactory)
    {
        _httpClient = employerUsersApiHttpClientFactory.CreateRestHttpClient();
    }

    public async Task<bool> IsEmailAddressInUse(string emailAddress, CancellationToken cancellationToken = default)
    {
        var employerUserEmailQueryUri = $"/api/user?email={emailAddress}";

        try
        {
            var user = await _httpClient.Get<UserDto>(employerUserEmailQueryUri, null, cancellationToken);
            return user != null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}