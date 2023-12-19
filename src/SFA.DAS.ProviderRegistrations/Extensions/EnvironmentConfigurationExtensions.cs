using System;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.ProviderRegistrations.Extensions
{
    public static class EnvironmentConfigurationExtensions
    {
        public static bool IsDev(this IConfiguration configuration)
        {
            var isDev = configuration["EnvironmentName"]?.StartsWith("DEV", StringComparison.CurrentCultureIgnoreCase) ?? false;
            var isDevelopment = configuration["EnvironmentName"]?.StartsWith("Development", StringComparison.CurrentCultureIgnoreCase) ?? false;

            return isDev || isDevelopment;
        }

        public static bool IsLocal(this IConfiguration configuration)
        {
            return configuration["EnvironmentName"]?.StartsWith("LOCAL", StringComparison.CurrentCultureIgnoreCase) ?? false;
        }
    }
}
