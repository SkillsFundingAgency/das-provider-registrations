using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions;

public static class ProviderUrlHelperExtensions
{
    public static string ProviderApprenticeshipServiceLink(this IUrlHelper helper, string path, ILinkGenerator linkGenerator)
    {
        return linkGenerator.ProviderApprenticeshipServiceLink(path);
    }
}