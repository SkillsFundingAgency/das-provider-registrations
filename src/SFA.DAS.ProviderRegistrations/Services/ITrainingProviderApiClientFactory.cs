using System.Net.Http;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface ITrainingProviderApiClientFactory
    {
        HttpClient CreateHttpClient();
    }
}
