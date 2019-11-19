using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand
{
    public class UpsertUserCommandHandler : AsyncRequestHandler<UpsertUserCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public UpsertUserCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(UpsertUserCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out _))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId) && i.Status < (int) InvitationStatus.AccountStarted, cancellationToken);
                invitation?.UpdateStatus((int) InvitationStatus.AccountStarted, DateTime.Now);
                await _db.Value.SaveChangesAsync(cancellationToken);
            }
        }
    }
}