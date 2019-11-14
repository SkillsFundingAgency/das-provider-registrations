namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public static class ProviderRegistrationsConfigurationKeys
    {
        public const string ProviderRegistrations = "SFA.DAS.ProviderRegistrations";
        public static string AuthenticationSettings = $"{ProviderRegistrations}:AuthenticationSettings";
        public static string ActiveDirectorySettings = $"{ProviderRegistrations}:ActiveDirectorySettings";
    }
}