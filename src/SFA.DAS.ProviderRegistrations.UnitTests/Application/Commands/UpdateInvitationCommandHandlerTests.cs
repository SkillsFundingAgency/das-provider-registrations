using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdateInvitationCommandHandlerTests
    {
        private Fixture fixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }


        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingUpdateInvitationCommand_ThenShouldUpdateInvitation(
           ProviderRegistrationsDbContext setupContext,
           ProviderRegistrationsDbContext confirmationContext,
           UpdateInvitationCommandHandler handler,
           UpdateInvitationCommand command)
        {
            //arrange
            var invitation = fixture.Create<Invitation>();
            invitation.UpdateInvitation(invitation.EmployerOrganisation, invitation.EmployerFirstName, invitation.EmployerLastName, (int)InvitationStatus.InvitationSent, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            command.CorrelationId = Guid.NewGuid().ToString();

            //act           
            await ((IRequestHandler<UpdateInvitationCommand>)handler).Handle(command, new CancellationToken());

            //assert
            var updatedInvite = await confirmationContext.Invitations.SingleAsync(s => s.Reference == invitation.Reference);
            updatedInvite.Status.Should().Be((int)InvitationStatus.InvitationSent);
            updatedInvite.EmployerOrganisation.Should().Be(invitation.EmployerOrganisation);
            updatedInvite.EmployerFirstName.Should().Be(invitation.EmployerFirstName);
            updatedInvite.EmployerLastName.Should().Be(invitation.EmployerLastName);
        }
    }
}

