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
            var invitationEvent = await (from invevnt in _db.Value.InvitationEvents
                                         join inv in _db.Value.Invitations on invevnt.Invitation.Id equals inv.Id
                                         where inv.Id == request.InvitationId
                                         select new InvitationEventDto
                                         {
                                             Date = invevnt.Date,
                                             EventType = invevnt.EventType,
                                             InvitationDto = new InvitationDto
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
                                                 Ukprn = inv.Ukprn
                                             }
                                         }).ToListAsync(cancellationToken);
         
            return new GetInvitationEventByIdQueryResult(invitationEvent);
        }
    }
}
