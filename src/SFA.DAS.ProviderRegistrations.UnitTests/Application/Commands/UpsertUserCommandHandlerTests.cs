using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    public class UpsertUserCommandHandlerTests
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
            UpsertUserCommandHandler handler)
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
            UpsertUserCommandHandler handler)
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
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            UpsertUserCommandHandler handler)
        {
            //arrange
            var updatedDate = DateTime.Now;
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now.AddHours(-1));
            setupContext.Invitations.Add(invitation);                       
            await setupContext.SaveChangesAsync();
            var command = new UpsertUserCommand(invitation.UserRef, updatedDate, invitation.Reference.ToString());

            //act
            await ((IRequestHandler<UpsertUserCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id && s.EventType == (int)EventType.AccountStarted);          
            addedInvitationEvent.Date.Should().Be(updatedDate);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            UpsertUserCommandHandler handler)
        {
            //arrange            
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new UpsertUserCommand(invitation.UserRef, DateTime.Now, invitation.Reference.ToString());

            //act
            try
            {
                await ((IRequestHandler<UpsertUserCommand, Unit>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation ID found for CorrelationId:{command.CorrelationId}");
            }
        }
    }
}