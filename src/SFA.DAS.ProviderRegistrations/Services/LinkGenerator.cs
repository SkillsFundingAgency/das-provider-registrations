using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Services;

public interface ILinkGenerator
{
    string ProviderApprenticeshipServiceLink(string path);
}

public class LinkGenerator : ILinkGenerator
{
    private readonly ProviderUrlConfiguration _providerConfiguration;

    public LinkGenerator(ProviderUrlConfiguration providerConfiguration)
    {
        _providerConfiguration = providerConfiguration;
    }

    public string ProviderApprenticeshipServiceLink(string path)
    {
        return Action(_providerConfiguration.ProviderApprenticeshipServiceBaseUrl, path);
    }
   
    private static string Action(string baseUrl, string path)
    {
        return baseUrl.TrimEnd('/') + "/" + path.Trim('/');
    }
}