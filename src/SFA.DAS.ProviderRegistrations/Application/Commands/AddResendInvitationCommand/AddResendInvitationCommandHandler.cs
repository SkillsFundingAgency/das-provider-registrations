using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand
{
    public class AddResendInvitationCommandHandler : AsyncRequestHandler<AddResendInvitationCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public AddResendInvitationCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }     

        protected override async Task Handle(AddResendInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitationEvents = new InvitationEvents(request.InvitationId, request.InvitationReSentDate, null, null, null);
            _db.Value.InvitationEvents.Add(invitationEvents);
            await _db.Value.SaveChangesAsync(cancellationToken);
        }
    }
}
