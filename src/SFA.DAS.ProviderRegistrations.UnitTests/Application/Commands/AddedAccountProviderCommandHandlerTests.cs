using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddedAccountProviderCommandTests
    {

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
            [Frozen] Lazy<ProviderRegistrationsDbContext> db,
            AddedAccountProviderCommandHandler handler,
            Invitation invitation)
        {
            //Arrange
            var command = new AddedAccountProviderCommand(invitation.Ukprn, Guid.NewGuid(), invitation.Reference.ToString());
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            db.Value.Invitations.Add(invitation);
            await db.Value.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedAccountProviderCommand, Unit>)handler).Handle(command, new CancellationToken());


            //assert
            var savedInvitation = await db.Value.Invitations.FirstAsync();
            savedInvitation.Status.Should().Be((int)InvitationStatus.InvitationComplete);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
            [Frozen] Lazy<ProviderRegistrationsDbContext> db,
            AddedAccountProviderCommandHandler handler,
            Invitation invitation)
        {
            //Arrange
            var command = new AddedAccountProviderCommand(12345, Guid.NewGuid(), Guid.NewGuid().ToString());
            db.Value.Invitations.Add(invitation);
            await db.Value.SaveChangesAsync();
            var statusBefore = invitation.Status;

            //act
            await ((IRequestHandler<AddedAccountProviderCommand, Unit>)handler).Handle(command, new CancellationToken());


            //assert
            // Confirm nothing has changed.
            var invite = await db.Value.Invitations.FirstAsync();
            invite.Status.Should().Be(statusBefore);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            [Frozen] Lazy<ProviderRegistrationsDbContext> db,
            AddedAccountProviderCommandHandler handler,
            AddedAccountProviderCommand command,
            Invitation invitation)
        {
            //Arrange
            db.Value.Invitations.Add(invitation);
            await db.Value.SaveChangesAsync();

            var statusBefore = invitation.Status;

            //act
            await ((IRequestHandler<AddedAccountProviderCommand, Unit>)handler).Handle(command, new CancellationToken());
            
            //assert
            // Confirm nothing has changed.
            var invite = await db.Value.Invitations.FirstAsync();
            invite.Status.Should().Be(statusBefore);
        }
    }
}