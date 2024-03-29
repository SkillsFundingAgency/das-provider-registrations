﻿using System.Linq;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;

public class GetInvitationEventByIdQueryHandler : IRequestHandler<GetInvitationEventByIdQuery, GetInvitationEventByIdQueryResult>
{
    private readonly Lazy<ProviderRegistrationsDbContext> _db;

    public GetInvitationEventByIdQueryHandler(Lazy<ProviderRegistrationsDbContext> db) => _db = db;

    public async Task<GetInvitationEventByIdQueryResult> Handle(GetInvitationEventByIdQuery request, CancellationToken cancellationToken)
    {
        var invitationEvent = await _db.Value.Invitations
            .Include(i => i.InvitationEvents)
            .Where(i => i.Id == request.InvitationId)
            .Select(inv => new InvitationDto
            {
                EmployerEmail = inv.EmployerEmail,
                EmployerFirstName = inv.EmployerFirstName,
                EmployerLastName = inv.EmployerLastName,
                EmployerOrganisation = inv.EmployerOrganisation,
                Id = inv.Id,
                ProviderOrganisationName = inv.ProviderOrganisationName,
                ProviderUserFullName = inv.ProviderUserFullName,
                Reference = inv.Reference,
                SentDate = inv.CreatedDate,
                Status = inv.Status,
                Ukprn = inv.Ukprn,
                Events = inv.InvitationEvents.Select(e => new InvitationEventDto
                {
                    Date = e.Date,
                    EventType = e.EventType
                })
            }).ToListAsync(cancellationToken);

        return new GetInvitationEventByIdQueryResult(invitationEvent.FirstOrDefault());
    }
}