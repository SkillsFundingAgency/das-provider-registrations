using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class AddedAccountProviderEventHandler : IHandleMessages<AddedAccountProviderEvent>
    {
        private readonly IMediator _mediator;

        public AddedAccountProviderEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(AddedAccountProviderEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new AddedAccountProviderCommand(message.ProviderUkprn, message.UserRef, message.CorrelationId.ToString(), message.Added));
        }
    }
}
