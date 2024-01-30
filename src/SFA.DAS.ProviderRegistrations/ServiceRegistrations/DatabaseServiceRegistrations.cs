﻿using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Extensions;

namespace SFA.DAS.ProviderRegistrations.ServiceRegistrations;

public static class DatabaseServiceRegistrations
{
    public static IServiceCollection AddDatabaseRegistrationWithNServiceBusTransaction(this IServiceCollection services, bool isDevOrLocal)
    {
        services.AddTransient<IProviderRegistrationsDbContextFactory, DbContextWithNServiceBusTransactionFactory>();
        
        services.AddDbContext<ProviderRegistrationsDbContext>((sp, options) =>
        {
            var factory = sp.GetService<IProviderRegistrationsDbContextFactory>();
            factory.CreateDbContext(isDevOrLocal);
        });

        services.AddScoped(provider => new Lazy<ProviderRegistrationsDbContext>(provider.GetService<ProviderRegistrationsDbContext>()));

        return services;
    }
    
    public static IServiceCollection AddDatabaseRegistration(this IServiceCollection services)
    {
        services.AddDbContext<ProviderRegistrationsDbContext>((sp, options) =>
        {
            var dbConnection = DatabaseExtensions.GetSqlConnection(sp.GetService<ProviderRegistrationsSettings>().DatabaseConnectionString);
            options.UseSqlServer(dbConnection);
        }, ServiceLifetime.Transient);

        services.AddScoped(provider => new Lazy<ProviderRegistrationsDbContext>(provider.GetService<ProviderRegistrationsDbContext>()));

        return services;
    }
}