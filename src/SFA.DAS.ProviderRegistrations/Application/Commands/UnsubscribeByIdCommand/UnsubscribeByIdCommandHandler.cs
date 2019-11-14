using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand
{
    public class UnsubscribeByIdCommandHandler : AsyncRequestHandler<UnsubscribeByIdCommand>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public UnsubscribeByIdCommandHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(UnsubscribeByIdCommand request, CancellationToken cancellationToken)
        {
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == request.CorrelationId, cancellationToken);

                if (invitation != null)
                {
                    var exists = await _db.Value.Unsubscribed.SingleOrDefaultAsync(u => u.EmailAddress == invitation.EmployerEmail && u.Ukprn == invitation.Ukprn, cancellationToken);

                    if (exists == null)
                    {
                        _db.Value.Unsubscribed.Add(new Unsubscribe(invitation.EmployerEmail, invitation.Ukprn));
                        await _db.Value.SaveChangesAsync(cancellationToken);
                    }
                }
            }
        }
    }
}