using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery
{
    public class GetProviderByUkprnQuery : IRequest<GetProviderByUkprnQueryResult>
    {
        public GetProviderByUkprnQuery(long ukprn)
        {
            Ukprn = ukprn;
        }

        public long Ukprn { get; }
    }
}