using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class SignedAgreementEventHandlerTests
    {
        [Test, DomainAutoData]
        public async Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendSignedAgreementCommand(
            TestableMessageHandlerContext context,
            [Frozen] Mock<IMediator> mediator,
            SignedAgreementEventHandler handler,
            SignedAgreementEvent message)
        {
            //arrange

            //act
            await handler.Handle(message,context);

            //assert
            mediator.Verify(s => s.Send(It.Is<SignedAgreementCommand>(c =>
                c.AccountId == message.AccountId &&
                c.AgreementId == message.AgreementId &&
                c.LegalEntityId == message.LegalEntityId &&
                c.OrganisationName == message.OrganisationName &&
                c.UserName == message.UserName &&
                c.UserRef == message.UserRef),It.IsAny<CancellationToken>()));
            
                //CB: To sort CorrelationId with new Package.
                // &&
                //c.CorrelationId == message.CorrelationId);
        }
    }
}