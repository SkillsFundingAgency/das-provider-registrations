using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;

public class UpsertedUserEventHandler : IHandleMessages<UpsertedUserEvent>
{
    private readonly IMediator _mediator;

    public UpsertedUserEventHandler(IMediator mediator)=> _mediator = mediator;

    public async Task Handle(UpsertedUserEvent message, IMessageHandlerContext context)
    {
        var command = new UpsertUserCommand(message.UserRef, message.Created, message.CorrelationId);
        
        await _mediator.Send(command);
    }
}