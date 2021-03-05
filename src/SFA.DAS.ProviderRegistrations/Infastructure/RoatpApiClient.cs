using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Infastructure
{
    public class RoatpApiClient
    {
        private readonly HttpClient _client;
        private readonly IWebConfiguration _config;

        public RoatpApiClient(HttpClient client, IWebConfiguration config)
        {
            _client = client
            _config = config;
        }

        public async Task<OrganisationSearchResult> GetOrganisationByUkprn(long ukprn)
        {
            var organisationSearchResults = await GetOrganisationSearchResultsFromRoatp(Convert.ToInt32(ukprn));
            return organisationSearchResults.FirstOrDefault();
        }

        private async Task<IEnumerable<OrganisationSearchResult>> GetOrganisationSearchResultsFromRoatp(int ukprn)
        {
            var apiResponse =
                await Get<OrganisationSearchResults>(
                    $"/api/v1/search?searchTerm={ukprn}");

            if (apiResponse?.SearchResults != null)
            {
                apiResponse.SearchResults = apiResponse.SearchResults
                    .Where(r => r.UKPRN.Equals(ukprn.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            var organisationSearchResults =
                Mapper
                    .Map<IEnumerable<Organisation>,
                        IEnumerable<OrganisationSearchResult>>(apiResponse?.SearchResults);
            return organisationSearchResults;
        }

        private async Task<T> Get<T>(string uri)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }

                return default(T);
            }
        }

        private string GetToken()
        {
            var tenantId = _config.RoatpApiAuthentication.TenantId;
            var clientId = _config.RoatpApiAuthentication.ClientId;
            var clientSecret = _config.RoatpApiAuthentication.ClientSecret;
            var resourceId = _config.RoatpApiAuthentication.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, (string)clientSecret);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;

            return result.AccessToken;
        }
    }
}