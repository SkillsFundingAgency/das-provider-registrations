using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand
{
    public class AddedAccountProviderCommandHandler : AsyncRequestHandler<AddedAccountProviderCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public AddedAccountProviderCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(AddedAccountProviderCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out _))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId) && i.Status < (int) InvitationStatus.InvitationComplete, cancellationToken);
                invitation?.UpdateStatus((int) InvitationStatus.InvitationComplete, DateTime.Now);
                await _db.Value.SaveChangesAsync();
            }
        }
    }
}