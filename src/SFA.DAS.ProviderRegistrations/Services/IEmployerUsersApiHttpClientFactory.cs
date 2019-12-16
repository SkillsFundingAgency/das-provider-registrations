using SFA.DAS.Http;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface IEmployerUsersApiHttpClientFactory
    {
        IRestHttpClient CreateRestHttpClient();
    }
}
