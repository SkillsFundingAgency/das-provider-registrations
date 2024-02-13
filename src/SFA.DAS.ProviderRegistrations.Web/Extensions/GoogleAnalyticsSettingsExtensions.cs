using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Startup;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class GoogleAnalyticsSettingsExtensions
    {
        public static IMvcBuilder AddGoogleAnalyticsSettings(this IMvcBuilder builder, IConfiguration configuration)
        {
            builder.EnableGoogleAnalytics();
            return builder;
        }
    }
}
