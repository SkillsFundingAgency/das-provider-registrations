using MediatR;
using SFA.DAS.ProviderRegistrations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByEmail
{
    public class GetInvitationByEmailQueryHandler : IRequestHandler<GetInvitationByEmailQuery, Guid>
    {
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public GetInvitationByEmailQueryHandler(Lazy<ProviderRegistrationsDbContext> db)
        {
            _db = db;
        }

        public async Task<Guid> Handle(GetInvitationByEmailQuery request, CancellationToken cancellationToken)
        {
            var correlationId =  _db.Value.Invitations
               .Where(i => i.EmployerEmail == request.EmailAddress).Select(x => x.Reference).SingleOrDefault();

            return correlationId;
        }
    }
}
