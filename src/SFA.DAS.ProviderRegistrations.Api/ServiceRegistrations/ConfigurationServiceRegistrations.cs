using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Api.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddApiConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ProviderRegistrationsSettings>(configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrations));

        var employerAccountsConfiguration = configuration.Get<ProviderRegistrationsSettings>();
        services.AddSingleton(employerAccountsConfiguration);

        // var encodingConfigJson = configuration.GetSection(ConfigurationKeys.EncodingConfig).Value;
        // var encodingConfig = JsonConvert.DeserializeObject<EncodingConfig>(encodingConfigJson);
        // services.AddSingleton(encodingConfig);

        return services;
    }
}