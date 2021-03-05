namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public class ProviderRegistrationsSettings 
    {
        public string DatabaseConnectionString { get; set; }

        public string RedisConnectionString { get; set; }

        public string RoatpApiClientBaseUrl { get; set; }

        public string EmployerAccountsBaseUrl { get; set; }
    }
}