using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery
{
    public class GetInvitationEventByIdQueryResult
    {
        public GetInvitationEventByIdQueryResult(InvitationEventsDto invitationEvent)
        {
            InvitationEvent = invitationEvent;
        }

        public InvitationEventsDto InvitationEvent { get; }
    }
}
