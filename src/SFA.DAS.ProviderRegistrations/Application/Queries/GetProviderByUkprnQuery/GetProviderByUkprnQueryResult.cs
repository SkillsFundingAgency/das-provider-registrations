namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery
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