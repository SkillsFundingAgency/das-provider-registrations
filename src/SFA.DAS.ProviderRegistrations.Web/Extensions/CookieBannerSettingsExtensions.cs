using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Provider.Shared.UI.Startup;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class CookieBannerSettingsExtensions
    {
        public static IMvcBuilder AddCookieBannerSettings(this IMvcBuilder builder, IConfiguration configuration)
        {
            builder.EnableCookieBanner();
            return builder;
        }
    }
}
