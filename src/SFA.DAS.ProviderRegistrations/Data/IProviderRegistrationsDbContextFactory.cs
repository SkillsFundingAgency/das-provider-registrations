namespace SFA.DAS.ProviderRegistrations.Data
{
    public interface IProviderRegistrationsDbContextFactory
    {
        ProviderRegistrationsDbContext CreateDbContext();
    }
}