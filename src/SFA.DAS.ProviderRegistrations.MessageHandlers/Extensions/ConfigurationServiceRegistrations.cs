using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ProviderRegistrationsSettings>(configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrations));

        services.AddSingleton(configuration.Get<ProviderRegistrationsSettings>());

        return services;
    }
}