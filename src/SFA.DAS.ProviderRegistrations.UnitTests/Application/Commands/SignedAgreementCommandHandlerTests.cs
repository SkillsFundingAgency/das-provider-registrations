using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
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
            TestSignedAgreementCommandDetails commandDetails)
        {
            //arrange            
            var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString());
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
            TestSignedAgreementCommandDetails commandDetails)
        {
            //arrange            
            var command = GetSignedAgreementCommand(commandDetails, string.Empty);
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
            TestSignedAgreementCommandDetails commandDetails)
        {
            //arrange
            var updateDate = DateTime.Now;
            var invitation = fixture.Create<Invitation>();
            var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString(), updateDate);
            setupContext.Invitations.Add(invitation);
            invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now.AddHours(-1));
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert            
            var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id && s.EventType == (int)EventType.LegalAgreementSigned);
            addedInvitationEvent.Date.Should().Be(updateDate);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
           ProviderRegistrationsDbContext setupContext,
           SignedAgreementCommandHandler handler,
           TestSignedAgreementCommandDetails commandDetails)
        {
            //arrange            
            var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString());
            setupContext.Invitations.Add(invitation);
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            await setupContext.SaveChangesAsync();

            //act
            try
            {
                await ((IRequestHandler<SignedAgreementCommand, Unit>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation ID found for CorrelationId:{command.CorrelationId}");
            }           
        }

        private SignedAgreementCommand GetSignedAgreementCommand(TestSignedAgreementCommandDetails details, string correlationId, DateTime? eventDate = null)
        {
            return new SignedAgreementCommand(
                details.AccountId,
                eventDate.HasValue ? eventDate.Value : details.EventDateTime,
                details.AgreementId,
                details.OrganisationName,
                details.LegalEntityId,
                details.CohortCreated,
                details.UserName,
                details.UserRef,
                correlationId);
        }

        public class TestSignedAgreementCommandDetails
        {
            public long AccountId { get; }
            public DateTime EventDateTime { get; }
            public string OrganisationName { get; }

            public long AgreementId { get; }

            public long LegalEntityId { get; }

            public bool CohortCreated { get; }

            public string UserName { get; }

            public Guid UserRef { get; }

            public string CorrelationId { get; }
        }
    }
}