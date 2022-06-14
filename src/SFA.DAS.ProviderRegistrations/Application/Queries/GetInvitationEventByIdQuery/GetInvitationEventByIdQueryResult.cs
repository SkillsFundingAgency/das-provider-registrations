using System;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery
{
    public class GetInvitationEventByIdQueryResult
    {
        public GetInvitationEventByIdQueryResult(InvitationDto invitation)
        {
            Invitation = invitation;
            EmployerOrganisation = Invitation?.EmployerOrganisation;
            InvitationSentDate = Invitation?.SentDate;
        }

        public InvitationDto Invitation { get; }
        public string EmployerOrganisation { get; }
        public DateTime? InvitationSentDate { get; }
    }
}
