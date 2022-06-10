using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
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
    public class AddedPayeSchemeCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
                ProviderRegistrationsDbContext setupContext,
                ProviderRegistrationsDbContext confirmationContext,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand command,
                Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            command.CorrelationId = invitation.Reference.ToString();
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvitation = await confirmationContext.Invitations.FirstAsync();
            savedInvitation.Status.Should().Be((int)InvitationStatus.PayeSchemeAdded);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedPayeSchemeCommandHandler handler,
            AddedPayeSchemeCommand command,
            Invitation invitation)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var statusBefore = invitation.Status;
            command.CorrelationId = invitation.Reference.ToString();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            // Confirm nothing has changed.
            var invite = await confirmationContext.Invitations.FirstAsync();
            invite.Status.Should().Be(statusBefore);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedPayeSchemeCommandHandler handler,
            AddedPayeSchemeCommand command,
            Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.LegalAgreementSigned, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            command.CorrelationId = invitation.Reference.ToString();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            // Confirm nothing has changed.
            var invite = await confirmationContext.Invitations.FirstAsync();
            invite.Status.Should().Be((int)InvitationStatus.LegalAgreementSigned);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
                ProviderRegistrationsDbContext setupContext,
                ProviderRegistrationsDbContext confirmationContext,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand command,
                Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            command.CorrelationId = invitation.Reference.ToString();
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.SingleAsync(s => s.InvitationId == invitation.Id);
            addedInvitationEvent.Date.Should().NotBeNull();
        }
    }
}