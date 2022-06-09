using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery
{
    public class GetInvitationEventByIdQueryHandler : IRequestHandler<GetInvitationEventByIdQuery, GetInvitationEventByIdQueryResult>
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public GetInvitationEventByIdQueryHandler(Lazy<ProviderRegistrationsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetInvitationEventByIdQueryResult> Handle(GetInvitationEventByIdQuery request, CancellationToken cancellationToken)
        {
            var invitationEvent = await _db.Value.InvitationEvents                
               .Where(i => i.InvitationId == request.InvitationId)
               .ProjectTo<InvitationEventsDto>(_configurationProvider)
               .SingleOrDefaultAsync(cancellationToken);
            
            var invitation = await _db.Value.Invitations
                .Where(i => i.Id == request.InvitationId)
                .SingleOrDefaultAsync(cancellationToken);

            invitationEvent.EmployerOrganisation = invitation.EmployerOrganisation;
            invitationEvent.InvitationSentDate = invitation.CreatedDate;

            return new GetInvitationEventByIdQueryResult(invitationEvent);
        }
    }
}
