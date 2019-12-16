using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery
{
    public class GetEmailAddressInUseQueryHandler : IRequestHandler<GetEmailAddressInUseQuery, bool>
    {
        private readonly IEmployerUsersService _api;
    
        public GetEmailAddressInUseQueryHandler(IEmployerUsersService api)
        {
            _api = api;
        }

        public async Task<bool> Handle(GetEmailAddressInUseQuery request, CancellationToken cancellationToken)
        {
            return await _api.IsEmailAddressInUse(request.EmailAddress, cancellationToken);
        }
    }
}