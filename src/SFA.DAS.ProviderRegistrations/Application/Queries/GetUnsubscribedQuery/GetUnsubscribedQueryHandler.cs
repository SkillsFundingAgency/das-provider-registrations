using SFA.DAS.ProviderRegistrations.Data;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;

public class GetUnsubscribedQueryHandler : IRequestHandler<GetUnsubscribedQuery, bool>
{
    private readonly Lazy<ProviderRegistrationsDbContext> _db;

    public GetUnsubscribedQueryHandler(Lazy<ProviderRegistrationsDbContext> db)
    {
        _db = db;
    }

    public async Task<bool> Handle(GetUnsubscribedQuery request, CancellationToken cancellationToken)
    {
        return await _db.Value.Unsubscribed.AnyAsync(u => u.Ukprn == request.Ukprn && u.EmailAddress == request.EmailAddress, cancellationToken);
    }
}