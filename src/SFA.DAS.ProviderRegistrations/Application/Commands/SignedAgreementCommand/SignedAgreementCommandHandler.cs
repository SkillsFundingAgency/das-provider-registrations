using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;

public class SignedAgreementCommandHandler : IRequestHandler<SignedAgreementCommand>
{
    private readonly Lazy<ProviderRegistrationsDbContext> _db;
    private readonly ILogger<SignedAgreementCommandHandler> _logger;

    public SignedAgreementCommandHandler(Lazy<ProviderRegistrationsDbContext> db, ILogger<SignedAgreementCommandHandler> logger)
    {
        _logger = logger;
        _db = db;
    }

    public async Task Handle(SignedAgreementCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out var correlationId))
        {
            var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == correlationId, cancellationToken);
            if (invitation == null) throw new InvalidInvitationException($"No invitation ID found for CorrelationId:{request.CorrelationId}");

            if (invitation.Status < (int)InvitationStatus.LegalAgreementSigned)
            {
                invitation.UpdateStatus((int)InvitationStatus.LegalAgreementSigned, request.EventDateTime);

                var invitationEvent = new InvitationEvent(invitation.Id, (int)EventType.LegalAgreementSigned, request.EventDateTime);
                invitation.InvitationEvents.Add(invitationEvent);

                await _db.Value.SaveChangesAsync(cancellationToken);
            }
            else
            {
                _logger.LogWarning($"Invitation status already: {((InvitationStatus)invitation.Status)} not going to store {EventType.LegalAgreementSigned} event");
            }
        }
    }
}