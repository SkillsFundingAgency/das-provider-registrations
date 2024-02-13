using System.Linq;
using SFA.DAS.Http;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Services;

public class ProviderService : IProviderService
{
    private IRestHttpClient _client;

    public ProviderService(IRoatpApiHttpClientFactory roatpApiHttpClientFactory)
    {
        _client = roatpApiHttpClientFactory.CreateRestHttpClient();
    }

    public async Task<Models.Provider> GetProvider(long ukprn)
    {
        var employerUserEmailQueryUri = $"/api/v1/Search?SearchTerm={ukprn}";
            
        var providerResult = await _client.Get<RoatpProviderResult>(employerUserEmailQueryUri);

        var provider = providerResult.SearchResults.FirstOrDefault();

        if (provider == null)
        {
            return null;
        }
            
        return new Models.Provider
        {
            Name = provider.Name,
            Ukprn = provider.Ukprn
        };
    }
}