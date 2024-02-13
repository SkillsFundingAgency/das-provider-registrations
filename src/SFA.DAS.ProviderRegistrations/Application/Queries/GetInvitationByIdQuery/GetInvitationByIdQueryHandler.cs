using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;

public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly Lazy<ProviderRegistrationsDbContext> _db;

    public GetInvitationByIdQueryHandler(Lazy<ProviderRegistrationsDbContext> db, IConfigurationProvider configurationProvider)
    {
        _db = db;
        _configurationProvider = configurationProvider;
    }

    public async Task<GetInvitationByIdQueryResult> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
    {
        var invitation = await _db.Value.Invitations
            .Where(i => i.Reference == request.CorrelationId)
            .ProjectTo<InvitationDto>(_configurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return invitation == null ? null : new GetInvitationByIdQueryResult(invitation);
    }
}