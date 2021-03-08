using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery
{
    public class GetProviderByUkprnQueryHandler : IRequestHandler<GetProviderByUkprnQuery, GetProviderByUkprnQueryResult>
    {
        private readonly IProviderService _api;
    
        public GetProviderByUkprnQueryHandler(IProviderService api)
        {
            _api = api;
        }

        public async Task<GetProviderByUkprnQueryResult> Handle(GetProviderByUkprnQuery request, CancellationToken cancellationToken)
        {
            var provider = await _api.GetProvider(request.Ukprn);
            return new GetProviderByUkprnQueryResult(provider.Name);
        }
    }
}