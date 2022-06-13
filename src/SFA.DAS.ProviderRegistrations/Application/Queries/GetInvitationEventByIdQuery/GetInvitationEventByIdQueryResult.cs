using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery
{
    public class GetInvitationEventByIdQueryResult
    {
        public GetInvitationEventByIdQueryResult(List<InvitationEventDto> invitationEvents, string employerOrganisation, DateTime? invitationSentDate)
        {
            InvitationEvents = invitationEvents;
            EmployerOrganisation = employerOrganisation;
            InvitationSentDate = invitationSentDate;
        }

        public List<InvitationEventDto> InvitationEvents { get; }
        public string EmployerOrganisation { get; }
        public DateTime? InvitationSentDate { get; }
    }
}
