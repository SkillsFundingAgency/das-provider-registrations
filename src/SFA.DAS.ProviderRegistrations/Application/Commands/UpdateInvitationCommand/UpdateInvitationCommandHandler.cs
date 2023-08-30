using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand
{
    public class UpdateInvitationCommandHandler : IRequestHandler<UpdateInvitationCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public UpdateInvitationCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(UpdateInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId), cancellationToken);            
            invitation?.UpdateInvitation(request.EmployerOrganisation, request.EmployerFirstName, request.EmployerLastName, (int)InvitationStatus.InvitationSent, DateTime.Now);
            await _db.Value.SaveChangesAsync(cancellationToken);
        }
    }
}
