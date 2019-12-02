using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public class EmployerUsersService : IEmployerUsersService
    {
        private readonly IRestHttpClient _httpClient;

        public EmployerUsersService(IEmployerUsersApiHttpClientFactory employerUsersApiHttpClientFactory)
        {
            _httpClient = employerUsersApiHttpClientFactory.CreateRestHttpClient();
        }

        public async Task<bool> IsEmailAddressInUse(string emailAddress, CancellationToken cancellationToken = default)
        {
            var employerUserEmailQueryUri = $"/api/users/email/{emailAddress}";

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
}