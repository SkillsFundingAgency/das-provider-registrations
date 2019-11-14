using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddedPayeSchemeCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
                [Frozen] Lazy<ProviderRegistrationsDbContext> db,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand command,
                Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            command.CorrelationId = invitation.Reference.ToString();
            db.Value.Invitations.Add(invitation);
            await db.Value.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvitation = await db.Value.Invitations.FirstAsync();
            savedInvitation.Status.Should().Be((int)InvitationStatus.PayeSchemeAdded);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
            [Frozen] Lazy<ProviderRegistrationsDbContext> db,
            AddedPayeSchemeCommandHandler handler,
            AddedPayeSchemeCommand command,
            Invitation invitation)
        {
            //arrange
            db.Value.Invitations.Add(invitation);
            await db.Value.SaveChangesAsync();
            var statusBefore = invitation.Status;
            command.CorrelationId = invitation.Reference.ToString();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            // Confirm nothing has changed.
            var invite = await db.Value.Invitations.FirstAsync();
            invite.Status.Should().Be(statusBefore);

        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            [Frozen] Lazy<ProviderRegistrationsDbContext> db,
            AddedPayeSchemeCommandHandler handler,
            AddedPayeSchemeCommand command,
            Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.LegalAgreementSigned, DateTime.Now);
            db.Value.Invitations.Add(invitation);
            await db.Value.SaveChangesAsync();
            command.CorrelationId = invitation.Reference.ToString();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            // Confirm nothing has changed.
            var invite = await db.Value.Invitations.FirstAsync();
            invite.Status.Should().Be((int)InvitationStatus.LegalAgreementSigned);
        }
    }
}