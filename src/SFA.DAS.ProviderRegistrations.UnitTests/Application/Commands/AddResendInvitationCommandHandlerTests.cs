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
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddResendInvitationCommandHandlerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingAddResendInvitationCommand_ThenShouldAddResendInvitation(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddResendInvitationCommandHandler handler,
            [Greedy]Invitation invitation)
        {
            //arrange
            var updatedDate = DateTime.Now;
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new AddResendInvitationCommand(invitation.Id, updatedDate);

            //act            
            await ((IRequestHandler<AddResendInvitationCommand>)handler).Handle(command, new CancellationToken());

            //assert
            confirmationContext.InvitationEvents.Should().ContainSingle(s => s.InvitationId == command.InvitationId && s.Date == updatedDate);            
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
           ProviderRegistrationsDbContext setupContext,           
           AddResendInvitationCommandHandler handler,
           [Greedy]Invitation invitation)
        {
            //arrange            
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new AddResendInvitationCommand(invitation.Id, DateTime.Now);

            //act
            try
            {
                await ((IRequestHandler<AddResendInvitationCommand>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.That($"No invitation found for InvitationId:{command.InvitationId}", Is.EqualTo(ex.Message));
            }
        }
    }
}
