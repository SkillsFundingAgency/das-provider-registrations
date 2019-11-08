using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class AddedPayeSchemeEventHandler : IHandleMessages<AddedPayeSchemeEvent>
    {
        private readonly IMediator _mediator;

        public AddedPayeSchemeEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(AddedPayeSchemeEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new AddedPayeSchemeCommand(message.AccountId, message.UserName, message.UserRef, message.PayeRef, message.Aorn, message.SchemeName, message.CorrelationId));
        }
    }
}
