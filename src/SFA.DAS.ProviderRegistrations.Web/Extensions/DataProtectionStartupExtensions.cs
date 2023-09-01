using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using StackExchange.Redis;


namespace SFA.DAS.ProviderRegistrations.Web.Extensions;

public static class DataProtectionStartupExtensions
{
    public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsDevOrLocal())
        {
            return services;
        }
            
        var redisConfiguration = configuration.GetSection(ProviderRegistrationsConfigurationKeys.RedisConnectionSettings).Get<RedisConnectionSettings>();

        if (redisConfiguration == null)
        {
            return services;
        }
        
        var redisConnectionString = redisConfiguration.RedisConnectionString;
        var dataProtectionKeysDatabase = redisConfiguration.DataProtectionKeysDatabase;

        var redis = ConnectionMultiplexer
            .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

        services.AddDataProtection()
            .SetApplicationName("das-provider-registrations")
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        return services;
    }
}