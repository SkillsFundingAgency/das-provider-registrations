namespace SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;

public class UnsubscribeByIdCommand : IRequest
{
    public UnsubscribeByIdCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
}