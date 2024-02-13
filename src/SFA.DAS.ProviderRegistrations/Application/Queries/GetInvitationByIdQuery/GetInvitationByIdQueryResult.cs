using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;

public class GetInvitationByIdQueryResult
{
    public GetInvitationByIdQueryResult(InvitationDto invitation)
    {
        Invitation = invitation;
    }

    public InvitationDto Invitation { get; }
}