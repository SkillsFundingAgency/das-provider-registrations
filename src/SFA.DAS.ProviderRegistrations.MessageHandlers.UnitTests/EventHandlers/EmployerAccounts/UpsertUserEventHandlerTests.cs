using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;
using SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class UpsertUserEventHandlerTests : FluentTest<UpsertUserEventHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendUpsertUserCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<UpsertUserCommand>((c, m) =>
                c.Created == m.Created &&
                c.CorrelationId == m.CorrelationId &&
                c.UserRef == m.UserRef));
        }
    }

    public class UpsertUserEventHandlerTestFixture : EventHandlerTestsFixture<UpsertedUserEvent, AddedAccountProviderEventHandler>
    {
    }
}