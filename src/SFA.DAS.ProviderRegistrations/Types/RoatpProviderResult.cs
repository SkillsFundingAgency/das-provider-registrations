using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Types;

public class RoatpProviderResult
{
    [JsonProperty("searchResults")]
    public List<OrganisationSearchResult> SearchResults { get; set; }
}
public class OrganisationSearchResult
{
    [JsonProperty("ukprn")]
    public int Ukprn { get; set; }
    [JsonIgnore]
    public string Name => LegalName ?? TradingName;
    [JsonProperty("tradingName")]
    public string TradingName { get; set; }
    [JsonProperty("legalName")]
    public string LegalName { get; set; }
}