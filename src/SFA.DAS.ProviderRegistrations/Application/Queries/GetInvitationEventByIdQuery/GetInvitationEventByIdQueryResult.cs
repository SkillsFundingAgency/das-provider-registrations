using System;
using System.Linq;
using System.Collections.Generic;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery
{
    public class GetInvitationEventByIdQueryResult
    {
        public GetInvitationEventByIdQueryResult(List<InvitationEventDto> invitationEvents)
        {
            InvitationEvents = invitationEvents;
            EmployerOrganisation = invitationEvents?.FirstOrDefault().InvitationDto.EmployerOrganisation;
            InvitationSentDate = invitationEvents?.FirstOrDefault().InvitationDto.SentDate;
        }

        public List<InvitationEventDto> InvitationEvents { get; }
        public string EmployerOrganisation { get; }
        public DateTime? InvitationSentDate { get; }
    }
}
