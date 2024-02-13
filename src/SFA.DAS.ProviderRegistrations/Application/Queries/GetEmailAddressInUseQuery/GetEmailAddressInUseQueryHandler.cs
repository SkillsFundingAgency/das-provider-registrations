using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;

public class GetEmailAddressInUseQueryHandler : IRequestHandler<GetEmailAddressInUseQuery, bool>
{
    private readonly IEmployerApprenticeshipService _api;
    
    public GetEmailAddressInUseQueryHandler(IEmployerApprenticeshipService api) => _api = api;

    public async Task<bool> Handle(GetEmailAddressInUseQuery request, CancellationToken cancellationToken)
    {
        return await _api.IsEmailAddressInUse(request.EmailAddress, cancellationToken);
    }
}