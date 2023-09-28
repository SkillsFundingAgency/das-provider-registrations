using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Services
{
    public class TrainingProviderApiClient : ITrainingProviderApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly TrainingProviderApiClientConfiguration _config;
        private readonly ILogger<TrainingProviderApiClient> _logger;

        public TrainingProviderApiClient(
            ITrainingProviderApiClientFactory factory,
            TrainingProviderApiClientConfiguration config,
            ILogger<TrainingProviderApiClient> logger)
        {
            _httpClient = factory.CreateHttpClient();
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Method to get the provider status from RoATP API by given ukprn number.
        /// </summary>
        /// <param name="providerId">ukprn number.</param>
        /// <returns>GetProviderStatusResult</returns>
        public async Task<GetProviderSummaryResult> GetProviderDetails(long providerId)
        {
            _logger.LogInformation("Getting Training Provider Details for ukprn:{0} returned OK", providerId);

            var url = $"{BaseUrl()}api/providers/{providerId}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    _logger.LogInformation("{Url} returned OK", url);
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<GetProviderSummaryResult>(json);
                case HttpStatusCode.NotFound:
                    _logger.LogInformation("{Url} returned not found status code", url);
                    return default;
                default:
                    _logger.LogError("{Url} returned unexpected status code", url);
                    return default;
            }
        }

        #region "Private Methods"
        private string BaseUrl()
        {
            if (_config.ApiBaseUrl.EndsWith("/"))
            {
                return _config.ApiBaseUrl;
            }
            return _config.ApiBaseUrl + "/";
        }
        #endregion
    }
}
