using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand
{
    public class AddedAccountProviderCommandHandler : IRequestHandler<AddedAccountProviderCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;
        private readonly ILogger<AddedAccountProviderCommandHandler> _logger;

        public AddedAccountProviderCommandHandler(Lazy<ProviderRegistrationsDbContext> db, ILogger<AddedAccountProviderCommandHandler> logger)
        {
            _logger = logger;
            _db = db;
        }

        public async Task Handle(AddedAccountProviderCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out var correlationId))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == correlationId, cancellationToken);
                if (invitation == null) throw new InvalidInvitationException($"No invitation ID found for CorrelationId:{request.CorrelationId}");

                if (invitation.Status < (int)InvitationStatus.AccountStarted)
                {
                    invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);

                    var invitationEvent = new InvitationEvent(invitation.Id, (int)EventType.AccountProviderAdded, request.EventDateTime);
                    invitation.InvitationEvents.Add(invitationEvent);

                    await _db.Value.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    _logger.LogWarning($"Invitation status already: {(InvitationStatus)invitation.Status} not going to store {EventType.AccountProviderAdded} event");
                }
                
            }
        }
    }
}