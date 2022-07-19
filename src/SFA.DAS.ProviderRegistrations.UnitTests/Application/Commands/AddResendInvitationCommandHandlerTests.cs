using AutoFixture;
using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System;
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
            AddResendInvitationCommandHandler handler)
        {
            //arrange
            var updatedDate = DateTime.Now;
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new AddResendInvitationCommand(invitation.Id, updatedDate);

            //act            
            await ((IRequestHandler<AddResendInvitationCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            confirmationContext.InvitationEvents.Should().ContainSingle(s => s.InvitationId == command.InvitationId && s.Date == updatedDate);            
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
           ProviderRegistrationsDbContext setupContext,           
           AddResendInvitationCommandHandler handler)
        {
            //arrange            
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new AddResendInvitationCommand(invitation.Id, DateTime.Now);

            //act
            try
            {
                await ((IRequestHandler<AddResendInvitationCommand, Unit>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation found for InvitationId:{command.InvitationId}");
            }
        }
    }
}
