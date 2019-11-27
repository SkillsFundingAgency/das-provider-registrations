using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprn
{
    public class GetProviderByUkprnQueryHandler : IRequestHandler<GetProviderByUkprnQuery, GetProviderByUkprnQueryResult>
    {
        private readonly IApprenticeshipInfoService _api;
    
        public GetProviderByUkprnQueryHandler(IApprenticeshipInfoService api)
        {
            _api = api;
        }

        public Task<GetProviderByUkprnQueryResult> Handle(GetProviderByUkprnQuery request, CancellationToken cancellationToken)
        {
            var provider = _api.GetProvider(request.Ukprn);
            return Task.FromResult(new GetProviderByUkprnQueryResult(provider.ProviderName));
        }
    }
}