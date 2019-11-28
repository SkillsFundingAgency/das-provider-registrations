namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprn
{
    public class GetProviderByUkprnQueryResult
    {
        public GetProviderByUkprnQueryResult(string providerName)
        {
            ProviderName = providerName;
        }

        public string ProviderName { get; }
    }
}