using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDasDistributedMemoryCache(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var redisConnectionString = configuration.GetSection(ProviderRegistrationsConfigurationKeys.RedisConnectionSettings).Get<RedisConnectionSettings>().RedisConnectionString;

        if (isDevelopment)
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(o => o.Configuration = redisConnectionString);
        }

        return services;
    }
}