namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;

public class GetInvitationEventByIdQuery : IRequest<GetInvitationEventByIdQueryResult>
{
    public GetInvitationEventByIdQuery(long invitationId)
    {
        InvitationId = invitationId;
    }

    public long InvitationId { get; }
}