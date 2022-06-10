using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery
{
    public class GetInvitationEventByIdQueryResult
    {
        public GetInvitationEventByIdQueryResult(List<InvitationEventDto> invitationEvent, string employerOrganisation, DateTime? invitationSentDate)
        {
            InvitationEvent = invitationEvent;
            EmployerOrganisation = employerOrganisation;
            InvitationSentDate = invitationSentDate;
        }

        public List<InvitationEventDto> InvitationEvent { get; }
        public string EmployerOrganisation { get; }
        public DateTime? InvitationSentDate { get; }
    }
}
