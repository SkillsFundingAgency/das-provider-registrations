using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddResendInvitationCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingAddResendInvitationCommand_ThenShouldAddResendInvitation(
            ProviderRegistrationsDbContext confirmationContext,
            AddResendInvitationCommandHandler handler,
            AddResendInvitationCommand command)
        {
            //arrange

            //act            
            await ((IRequestHandler<AddResendInvitationCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            confirmationContext.InvitationEvents.Single(s => s.InvitationId == command.InvitationId).InvitationReSentDate.Should().NotBeNull();                        
        }
    }
}
