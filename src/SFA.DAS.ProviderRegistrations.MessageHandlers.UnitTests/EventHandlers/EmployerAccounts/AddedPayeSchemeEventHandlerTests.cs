using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts;

[TestFixture]
[Parallelizable]
public class AddedPayeSchemeEventHandlerTests
{
    [Test, DomainAutoData]
    public async Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendAddedPayeSchemeCommand(
        TestableMessageHandlerContext context,
        [Frozen] Mock<IMediator> mediator,
        AddedPayeSchemeEventHandler handler,
        AddedPayeSchemeEvent message)
    {
        //arrange

        //act
        await handler.Handle(message, context);

        //assert
        mediator.Verify(s => s.Send(It.Is<AddedPayeSchemeCommand>(c =>
            c.AccountId == message.AccountId &&
            c.Aorn == message.Aorn &&
            c.CorrelationId == message.CorrelationId &&
            c.PayeRef == message.PayeRef &&
            c.SchemeName == message.SchemeName &&
            c.UserName == message.UserName &&
            c.UserRef == message.UserRef), It.IsAny<CancellationToken>()));
    }
}