using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class SignedAgreementCommandHandlerTests
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
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            SignedAgreementCommandHandler handler,
            SignedAgreementCommand command)
        {
            //arrange            
            command.CorrelationId = invitation.Reference.ToString();
            setupContext.Invitations.Add(invitation);
            invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now);
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
            SignedAgreementCommand command)
        {
            //arrange            
            db.Invitations.Add(invitation);
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            await db.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvite = await confirmationContext.Invitations.FirstAsync(i => i.Reference == invitation.Reference);
            savedInvite.Status.Should().Be((int)InvitationStatus.InvitationSent);
        }      

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            SignedAgreementCommandHandler handler,
            SignedAgreementCommand command)
        {
            //arrange
            var invitation = fixture.Create<Invitation>();
            command.CorrelationId = invitation.Reference.ToString();
            setupContext.Invitations.Add(invitation);
            invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert            
            var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id);
            addedInvitationEvent.Date.Should().NotBeNull();
        }
    }
}