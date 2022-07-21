using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand
{
    public class AddedPayeSchemeCommandHandler : AsyncRequestHandler<AddedPayeSchemeCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public AddedPayeSchemeCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(AddedPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out _))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId) && i.Status < (int) InvitationStatus.PayeSchemeAdded, cancellationToken);
                if (invitation == null) throw new InvalidInvitationException($"No invitation ID found for CorrelationId:{ request.CorrelationId}");
                invitation.UpdateStatus((int) InvitationStatus.PayeSchemeAdded, DateTime.Now);

                var invitationEvent = new InvitationEvent(invitation.Id, (int)EventType.PayeSchemeAdded, request.EventDateTime);
                invitation.InvitationEvents.Add(invitationEvent);
              
                await _db.Value.SaveChangesAsync(cancellationToken);
            }
        }
    }
}