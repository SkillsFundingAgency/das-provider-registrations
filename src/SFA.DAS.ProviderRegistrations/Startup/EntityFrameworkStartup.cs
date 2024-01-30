using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Services.AppAuthentication;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.SqlServer.Data;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.Startup;

public static class EntityFrameworkStartup
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, ProviderRegistrationsSettings config)
    {
        services.AddScoped<ProviderRegistrationsDbContext>(p =>
        {
            var unitOfWorkContext = p.GetService<IUnitOfWorkContext>();
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            ProviderRegistrationsDbContext dbContext;
            try
            {                    
                var synchronizedStorageSession = unitOfWorkContext.Get<SynchronizedStorageSession>();
                var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
                var optionsBuilder = new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseSqlServer(sqlStorageSession.Connection);                    
                dbContext = new ProviderRegistrationsDbContext(sqlStorageSession.Connection, config, optionsBuilder.Options, azureServiceTokenProvider);
                dbContext.Database.UseTransaction(sqlStorageSession.Transaction);
            }
            catch (KeyNotFoundException)
            {
                var connection = DatabaseExtensions.GetSqlConnection(config.DatabaseConnectionString);
                var optionsBuilder = new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseSqlServer(connection);
                dbContext = new ProviderRegistrationsDbContext(optionsBuilder.Options);
            }

            return dbContext;
        });

        services.AddScoped(provider => new Lazy<ProviderRegistrationsDbContext>(provider.GetService<ProviderRegistrationsDbContext>()));

        return services;
    }
}