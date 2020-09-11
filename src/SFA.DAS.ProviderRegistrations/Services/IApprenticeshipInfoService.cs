namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface IApprenticeshipInfoService
    {
        Apprenticeships.Api.Types.Providers.Provider GetProvider(long ukprn);
    }
}
