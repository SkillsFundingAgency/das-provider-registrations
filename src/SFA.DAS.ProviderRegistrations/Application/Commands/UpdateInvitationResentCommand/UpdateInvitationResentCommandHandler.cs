using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationResentCommand
{
    public class UpdateInvitationResentCommandHandler : AsyncRequestHandler<UpdateInvitationResentCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public UpdateInvitationResentCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }      

        protected override async Task Handle(UpdateInvitationResentCommand request, CancellationToken cancellationToken)
        {
            var invitationEvents = new InvitationEvents(request.InvitationId, request.InvitationReSentDate, null, null, null);
            _db.Value.InvitationEvents.Add(invitationEvents);
            await _db.Value.SaveChangesAsync(cancellationToken);
        }
    }
}
