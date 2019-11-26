using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprn
{
    public class GetProviderByUkprnQueryResult
    {
        public GetProviderByUkprnQueryResult(Provider provider)
        {
            ProviderName = provider.ProviderName;
        }

        public string ProviderName { get; }
    }
}