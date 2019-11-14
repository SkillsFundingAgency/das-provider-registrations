using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class AddedAccountProviderEventHandler : IHandleMessages<UpsertedUserEvent>
    {
        private readonly IMediator _mediator;

        public AddedAccountProviderEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpsertedUserEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new UpsertUserCommand(message.UserRef, message.Created, message.CorrelationId));
        }
    }
}
