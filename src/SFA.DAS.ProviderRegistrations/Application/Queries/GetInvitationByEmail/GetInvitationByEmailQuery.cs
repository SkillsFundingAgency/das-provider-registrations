using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByEmail
{
    public class GetInvitationByEmailQuery : IRequest<Guid>
    {
        public GetInvitationByEmailQuery(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
    }
}
