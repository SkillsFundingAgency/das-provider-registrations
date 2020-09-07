using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class ZenDeskExtensions
    {
        public static IMvcBuilder AddZenDeskSettings(this IMvcBuilder builder, IConfiguration config)
        {
            var zenDeskConfiguration = config.GetSection(ProviderRegistrationsConfigurationKeys.ZenDeskSettings).Get<ZenDeskConfiguration>();
            builder.SetZenDeskConfiguration(zenDeskConfiguration);

            return builder;
        }
    }
}