using Microsoft.Extensions.Configuration;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Extensions;
using SFA.DAS.ProviderRegistrations.Web.Filters;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

public static class MvcServiceRegistrations
{
    public static IServiceCollection AddDasMvc(this IServiceCollection services, IConfiguration configuration)
    {
        var useDfESignIn = configuration.GetSection(ProviderRegistrationsConfigurationKeys.UseDfESignIn).Get<bool>();
        
        services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new GoogleAnalyticsFilter());
                options.Filters.Add(new AuthorizeFilter(PolicyNames.ProviderPolicyName));
                options.AddAuthorization();
            })
            .AddNavigationBarSettings(configuration)
            .AddZenDeskSettings(configuration)
            .AddGoogleAnalyticsSettings(configuration)
            .AddCookieBannerSettings(configuration)
            .AddControllersAsServices()
            .AddSessionStateTempDataProvider()
            .SetDfESignInConfiguration(useDfESignIn);
        
        return services;
    }
}