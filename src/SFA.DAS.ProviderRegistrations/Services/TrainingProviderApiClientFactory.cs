using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SFA.DAS.ProviderRegistrations.Services
{
    ///<inheritdoc cref="ITrainingProviderApiClientFactory"/>
    public class TrainingProviderApiClientFactory : ITrainingProviderApiClientFactory
    {
        private readonly HttpClient _httpClient;
        private readonly TrainingProviderApiClientConfiguration _settings;
        private readonly IConfiguration _configuration;

        public TrainingProviderApiClientFactory(
            HttpClient client,
            TrainingProviderApiClientConfiguration settings,
            IConfiguration configuration)
        {
            _settings = settings;
            _configuration = configuration;
            _httpClient = client;
        }
        public HttpClient CreateHttpClient()
        {
            return GetHttpClient(_settings, _configuration);
        }

        ///<inherit-doc />
        private HttpClient GetHttpClient(
            IManagedIdentityClientConfiguration apiClientConfiguration,
            IConfiguration config)
        {
            if (!config.IsLocal())
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = azureServiceTokenProvider.GetAccessTokenAsync(apiClientConfiguration.IdentifierUri).GetAwaiter().GetResult();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            _httpClient.DefaultRequestHeaders.Remove("X-Version");
            _httpClient.DefaultRequestHeaders.Add("X-Version", "1.0");
            return _httpClient;
        }
    }
}
