using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class AddedPayeSchemeEventHandlerTests : FluentTest<AddedPayeSchemeEventHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendAddedPayeSchemeCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<AddedPayeSchemeCommand>((c, m) =>
                c.AccountId == m.AccountId &&
                c.Aorn == m.Aorn &&
                c.CorrelationId == m.CorrelationId &&
                c.PayeRef == m.PayeRef &&
                c.SchemeName == m.SchemeName &&
                c.UserName == m.UserName &&
                c.UserRef == m.UserRef));
        }
    }

    public class AddedPayeSchemeEventHandlerTestFixture : EventHandlerTestsFixture<AddedPayeSchemeEvent, AddedPayeSchemeEventHandler>
    {
    }
}