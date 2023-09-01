namespace SFA.DAS.ProviderRegistrations.Configuration;

public class ProviderRegistrationsSettings 
{
    public string DatabaseConnectionString { get; set; }

    public string EmployerAccountsBaseUrl { get; set; }
    public bool UseGovLogin { get; set; }
    public string ResourceEnvironmentName { get; set; }
}