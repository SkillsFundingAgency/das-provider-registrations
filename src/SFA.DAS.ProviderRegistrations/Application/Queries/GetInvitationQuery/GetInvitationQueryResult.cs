using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;

public class GetInvitationQueryResult
{
    public GetInvitationQueryResult(List<InvitationDto> invitations)
    {
        Invitations = invitations;
    }

    public List<InvitationDto> Invitations { get; }
}