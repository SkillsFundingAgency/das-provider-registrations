using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;

public class SignedAgreementEventHandler : IHandleMessages<SignedAgreementEvent>
{
    private readonly IMediator _mediator;

    public SignedAgreementEventHandler(IMediator mediator) => _mediator = mediator;

    public Task Handle(SignedAgreementEvent message, IMessageHandlerContext context)
    {
        var command = new SignedAgreementCommand(
            message.AccountId,
            message.Created,
            message.AgreementId,
            message.OrganisationName,
            message.LegalEntityId,
            message.CohortCreated,
            message.UserName,
            message.UserRef,
            message.CorrelationId);
        
        return _mediator.Send(command);
    }
}