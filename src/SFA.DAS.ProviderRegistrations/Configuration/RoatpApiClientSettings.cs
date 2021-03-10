using Newtonsoft.Json;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public class RoatpApiClientSettings : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set;}

    }
}