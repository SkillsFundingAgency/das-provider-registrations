using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public class TrainingProviderApiClientConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}
