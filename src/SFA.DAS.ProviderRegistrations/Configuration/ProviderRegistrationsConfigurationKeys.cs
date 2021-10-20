namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public static class ProviderRegistrationsConfigurationKeys
    {
        public const string ProviderRegistrations = "SFA.DAS.ProviderRegistrations";
        public static string AuthenticationSettings = $"{ProviderRegistrations}:AuthenticationSettings";
        public static string ActiveDirectorySettings = $"{ProviderRegistrations}:ActiveDirectorySettings";
        public static string NServiceBusSettings = $"{ProviderRegistrations}:NServiceBusSettings";
        public static string EmployerApprenticeshipApiClientSettings = $"{ProviderRegistrations}:EmployerApprenticeshipApiClientSettings";
        public static string ZenDeskSettings = $"{ProviderRegistrations}:ZenDeskSettings";
        public static string RedisConnectionSettings = $"{ProviderRegistrations}:RedisConnectionSettings";
        public static string RoatpApiClientSettings =$"{ProviderRegistrations}:RoatpApiClientSettings";
        public static string ProviderSharedUIConfiguration = $"ProviderSharedUIConfiguration";
    }
}