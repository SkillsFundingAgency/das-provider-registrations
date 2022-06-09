using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
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
    public class SignedAgreementCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            SignedAgreementCommandHandler handler,
            SignedAgreementCommand command,
            Invitation invite)
        {
            //arrange
            command.CorrelationId = invite.Reference.ToString();
            setupContext.Invitations.Add(invite);
            invite.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvite = await confirmationContext.Invitations.FirstAsync(i => i.Reference.ToString() == command.CorrelationId);
            savedInvite.Status.Should().Be((int)InvitationStatus.LegalAgreementSigned);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext db,
            ProviderRegistrationsDbContext confirmationContext,
            SignedAgreementCommandHandler handler,
            SignedAgreementCommand command,
            Invitation invite)
        {
            //arrange
            db.Invitations.Add(invite);
            invite.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            await db.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvite = await confirmationContext.Invitations.FirstAsync(i => i.Reference == invite.Reference);
            savedInvite.Status.Should().Be((int)InvitationStatus.InvitationSent);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            SignedAgreementCommandHandler handler,
            SignedAgreementCommand command,
            Invitation invite)
        {
            //arrange
            command.CorrelationId = invite.Reference.ToString();
            setupContext.Invitations.Add(invite);
            invite.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvite = await confirmationContext.Invitations.FirstAsync(i => i.Reference.ToString() == command.CorrelationId);
            savedInvite.Status.Should().Be((int)InvitationStatus.InvitationComplete);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            SignedAgreementCommandHandler handler,
            SignedAgreementCommand command,
            Invitation invite)
        {
            //arrange
            command.CorrelationId = invite.Reference.ToString();
            setupContext.Invitations.Add(invite);
            invite.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.SingleAsync(s => s.InvitationId == invite.Id);
            addedInvitationEvent.AgreementAcceptedDate.Should().NotBeNull();
        }
    }
}