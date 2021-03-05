namespace SFA.DAS.ProviderRegistrations.Services
{
    public interface IApprenticeshipInfoService
    {
        Roatp.Api.Types.Provider GetProvider(long ukprn);
    }
}
