using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
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
                invitation?.UpdateStatus((int) InvitationStatus.PayeSchemeAdded, DateTime.Now);

                var invitationEvents = new InvitationEvent(invitation?.Id, (int)EventType.PayeSchemeAdded, DateTime.UtcNow);
                _db.Value.InvitationEvents.Add(invitationEvents);

                await _db.Value.SaveChangesAsync(cancellationToken);
            }
        }
    }
}