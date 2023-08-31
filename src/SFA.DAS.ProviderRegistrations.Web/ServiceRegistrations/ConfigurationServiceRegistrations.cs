using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddWebConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ProviderRegistrationsSettings>(configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrations));

        var providerRegistrationsSettings = configuration.Get<ProviderRegistrationsSettings>();
        services.AddSingleton(providerRegistrationsSettings);
        
        services.AddConfiguration<AuthenticationSettings>(configuration, ProviderRegistrationsConfigurationKeys.AuthenticationSettings);
        services.AddConfiguration<ActiveDirectorySettings>(configuration, ProviderRegistrationsConfigurationKeys.ActiveDirectorySettings);
        services.AddConfiguration<ProviderRegistrationsSettings>(configuration, ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings);
        services.AddConfiguration<NServiceBusSettings>(configuration, ProviderRegistrationsConfigurationKeys.NServiceBusSettings);
        services.AddConfiguration<EmployerApprenticeshipApiClientSettings>(configuration, ProviderRegistrationsConfigurationKeys.EmployerApprenticeshipApiClientSettings);
        services.AddConfiguration<ZenDeskConfiguration>(configuration, ProviderRegistrationsConfigurationKeys.ZenDeskSettings);
        services.AddConfiguration<RoatpApiClientSettings>(configuration, ProviderRegistrationsConfigurationKeys.RoatpApiClientSettings);
        
        return services;
    }

    private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration,  string key) where T : class
    {
        services.Configure<T>(configuration.GetSection(key));
        services.AddSingleton(cfg => cfg.GetService<IOptions<T>>().Value);
    }
}