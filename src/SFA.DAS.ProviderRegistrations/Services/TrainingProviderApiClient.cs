using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Services
{
    ///<inheritdoc cref="ITrainingProviderApiClient"/>
    public class TrainingProviderApiClient : ITrainingProviderApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly TrainingProviderApiClientConfiguration _config;

        public TrainingProviderApiClient(
            HttpClient httpClient,
            TrainingProviderApiClientConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        ///<inherit-doc />
        public async Task<GetProviderSummaryResult> GetProviderDetails(long providerId)
        {
            var url = $"{BaseUrl()}api/providers/{providerId}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            await AddAuthenticationHeader(requestMessage);

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<GetProviderSummaryResult>(json);
                case HttpStatusCode.NotFound:
                default:
                    return default;
            }
        }

        private string BaseUrl()
        {
            if (_config.ApiBaseUrl.EndsWith("/"))
            {
                return _config.ApiBaseUrl;
            }
            return _config.ApiBaseUrl + "/";
        }
        private async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (!string.IsNullOrEmpty(_config.IdentifierUri))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_config.IdentifierUri);
                httpRequestMessage.Headers.Remove("X-Version");
                httpRequestMessage.Headers.Add("X-Version", "1.0");
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
