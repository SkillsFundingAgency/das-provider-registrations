using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdateInvitationCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingUpdateInvitationCommand_ThenShouldUpdateReference(
           ProviderRegistrationsDbContext setupContext,
           ProviderRegistrationsDbContext confirmationContext,
           UpdateInvitationCommandHandler handler,
           UpdateInvitationCommand command,
           Invitation invitation)
        {
            //arrange
            invitation.UpdateInvitation(invitation.EmployerOrganisation, invitation.EmployerFirstName, invitation.EmployerLastName, (int)InvitationStatus.InvitationSent, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            command.CorrelationId = Guid.NewGuid().ToString();

            //act           
            var result = await ((IRequestHandler<UpdateInvitationCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var updatedInvite = await confirmationContext.Invitations.SingleAsync(s => s.Reference == invitation.Reference);
            updatedInvite.Status.Should().Be((int)InvitationStatus.InvitationSent);
            updatedInvite.EmployerOrganisation.Should().Be(invitation.EmployerOrganisation);
            updatedInvite.EmployerFirstName.Should().Be(invitation.EmployerFirstName);
            updatedInvite.EmployerLastName.Should().Be(invitation.EmployerLastName);
        }
    }
}

