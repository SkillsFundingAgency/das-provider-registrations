using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpsertUserCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            UpsertUserCommandHandler handler,
            Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new UpsertUserCommand(invitation.UserRef, DateTime.Now, invitation.Reference.ToString());

            //act
            await ((IRequestHandler<UpsertUserCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var updatedInvite = await confirmationContext.Invitations.SingleAsync(s => s.Reference == invitation.Reference);
            updatedInvite.Status.Should().Be((int)InvitationStatus.AccountStarted);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            UpsertUserCommandHandler handler,
            Invitation invitation)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var statusBefore = invitation.Status;
            var command = new UpsertUserCommand(invitation.UserRef, DateTime.Now, "unknownreference");

            //act
            await ((IRequestHandler<UpsertUserCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var dbInvite = await confirmationContext.Invitations.FirstAsync(f => f.Reference == invitation.Reference);
            dbInvite.Status.Should().Be(statusBefore);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            UpsertUserCommandHandler handler,
            Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new UpsertUserCommand(invitation.UserRef, DateTime.Now, invitation.Reference.ToString());

            //act
            await ((IRequestHandler<UpsertUserCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var updatedInvite = await confirmationContext.Invitations.SingleAsync(s => s.Reference == invitation.Reference);
            updatedInvite.Status.Should().Be((int)InvitationStatus.InvitationComplete);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            UpsertUserCommandHandler handler,
            Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            setupContext.Invitations.Add(invitation);                       
            await setupContext.SaveChangesAsync();
            var command = new UpsertUserCommand(invitation.UserRef, DateTime.Now, invitation.Reference.ToString());

            //act
            await ((IRequestHandler<UpsertUserCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.SingleAsync(s => s.InvitationId == invitation.Id);
            addedInvitationEvent.Date.Should().NotBeNull();
        }

    }
}