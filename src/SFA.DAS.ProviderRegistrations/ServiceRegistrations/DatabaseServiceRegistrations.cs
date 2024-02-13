using Microsoft.Extensions.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Extensions;

namespace SFA.DAS.ProviderRegistrations.ServiceRegistrations;

public static class DatabaseServiceRegistrations
{
    public static IServiceCollection AddDatabaseRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var providerRegistrationsSettings = configuration
            .GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings)
            .Get<ProviderRegistrationsSettings>();

        services.AddDbContext<ProviderRegistrationsDbContext>((sp, options) =>
        {
            var dbConnection = DatabaseExtensions.GetSqlConnection(providerRegistrationsSettings.DatabaseConnectionString);
            options.UseSqlServer(dbConnection);
        }, ServiceLifetime.Transient);

        services.AddScoped(provider => new Lazy<ProviderRegistrationsDbContext>(provider.GetService<ProviderRegistrationsDbContext>()));

        return services;
    }
}