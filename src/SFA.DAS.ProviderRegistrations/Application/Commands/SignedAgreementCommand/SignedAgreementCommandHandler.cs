using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand
{
    public class SignedAgreementCommandHandler : AsyncRequestHandler<SignedAgreementCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public SignedAgreementCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(SignedAgreementCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out _))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId) && i.Status < (int) InvitationStatus.LegalAgreementSigned, cancellationToken);
                if (invitation == null) throw new InvalidInvitationException($"No invitation ID found for CorrelationId:{ request.CorrelationId}");
                invitation.UpdateStatus((int) InvitationStatus.LegalAgreementSigned, request.EventDateTime);

                var invitationEvent = new InvitationEvent(invitation.Id, (int)EventType.LegalAgreementSigned, request.EventDateTime);
                invitation.InvitationEvents.Add(invitationEvent);

                await _db.Value.SaveChangesAsync(cancellationToken);
            }
        }
    }
}