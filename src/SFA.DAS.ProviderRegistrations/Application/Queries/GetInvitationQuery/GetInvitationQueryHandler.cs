using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery
{
    public class GetInvitationQueryHandler : IRequestHandler<GetInvitationQuery, GetInvitationQueryResult>
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Lazy<ProviderRegistrationsDbContext> _db;

        public GetInvitationQueryHandler(Lazy<ProviderRegistrationsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetInvitationQueryResult> Handle(GetInvitationQuery request, CancellationToken cancellationToken)
        {
            var invitations = await _db.Value.Invitations
                .Where(i => i.Ukprn == request.Ukprn)
                .OrderBy($"{request.SortColumn} {request.SortDirection}")
                .ThenBy(request.SecondarySortColumn)
                .ProjectTo<InvitationDto>(_configurationProvider)
                .ToListAsync(cancellationToken);

            return new GetInvitationQueryResult(invitations);
        }
    }
}