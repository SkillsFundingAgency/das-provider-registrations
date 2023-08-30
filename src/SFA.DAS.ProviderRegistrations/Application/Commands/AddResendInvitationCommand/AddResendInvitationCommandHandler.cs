using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand
{
    public class AddResendInvitationCommandHandler : IRequestHandler<AddResendInvitationCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public AddResendInvitationCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }     

        public async Task Handle(AddResendInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Id == request.InvitationId, cancellationToken);
            if (invitation == null) throw new InvalidInvitationException($"No invitation found for InvitationId:{ request.InvitationId}");
            var invitationEvent = new InvitationEvent(invitation.Id, (int)EventType.InvitationResent, request.InvitationReSentDate);
            invitation.InvitationEvents.Add(invitationEvent);

            await _db.Value.SaveChangesAsync(cancellationToken);
        }
    }
}
