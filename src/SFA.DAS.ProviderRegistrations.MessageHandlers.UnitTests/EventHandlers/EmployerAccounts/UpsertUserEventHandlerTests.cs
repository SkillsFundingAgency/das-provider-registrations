using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;
using SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts;

[TestFixture]
[Parallelizable]
public class UpsertUserEventHandlerTests
{
    [Test, DomainAutoData]
    public async Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendUpsertUserCommand(
        TestableMessageHandlerContext context,
        [Frozen] Mock<IMediator> mediator,
        UpsertedUserEventHandler handler,
        UpsertedUserEvent message)
    {
        //arrange

        //act
        await handler.Handle(message, context);

        //assert
        mediator.Verify(s => s.Send(It.Is<UpsertUserCommand>(c =>
            c.EventDateTime == message.Created &&
            c.CorrelationId == message.CorrelationId &&
            c.UserRef == message.UserRef), It.IsAny<CancellationToken>()));
    }
}