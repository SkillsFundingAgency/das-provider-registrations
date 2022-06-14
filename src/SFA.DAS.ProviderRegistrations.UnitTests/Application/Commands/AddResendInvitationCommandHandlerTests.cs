using AutoFixture;
using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
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
        private Fixture fixture { get; set; }
        private Invitation invitation { get; set; }

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            invitation = fixture.Create<Invitation>();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingAddResendInvitationCommand_ThenShouldAddResendInvitation(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddResendInvitationCommandHandler handler,
            AddResendInvitationCommand command)
        {
            //arrange            
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            command.InvitationId = invitation.Id;

            //act            
            await ((IRequestHandler<AddResendInvitationCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            //TODO : CHECK confirmationContext.InvitationEvents.Single(s => s.Invitation.Id == command.InvitationId).Date.Should().NotBeNull();
            confirmationContext.InvitationEvents.FirstOrDefault(s => s.Invitation.Id == command.InvitationId).Date.Should().NotBeNull();
        }
    }
}
