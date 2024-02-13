namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;

public class GetInvitationByIdQuery : IRequest<GetInvitationByIdQueryResult>
{
    public GetInvitationByIdQuery(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
}