using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddedAccountProviderCommandTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedAccountProviderCommandHandler handler,
            [Greedy]Invitation invitation)
        {
            //Arrange            
            var command = new AddedAccountProviderCommand(invitation.Ukprn, Guid.NewGuid(), invitation.Reference.ToString(), DateTime.Now);
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedAccountProviderCommand>)handler).Handle(command, new CancellationToken());


            //assert
            var savedInvitation = await confirmationContext.Invitations.FirstAsync();
            savedInvitation.Status.Should().Be((int)InvitationStatus.InvitationComplete);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedAccountProviderCommandHandler handler,
            [Greedy]Invitation invitation)
        {
            //arrange
            var updateDate = DateTime.Now;
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now.AddHours(-1));
            var command = new AddedAccountProviderCommand(invitation.Ukprn, Guid.NewGuid(), invitation.Reference.ToString(), updateDate);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedAccountProviderCommand>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.InvitationId == invitation.Id && s.EventType == (int)EventType.AccountProviderAdded);
            addedInvitationEvent.Date.Should().Be(updateDate);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedAccountProviderCommandHandler handler,
            [Greedy]Invitation invitation)
        {
            invitation.InvitationEvents.Clear();
            
            //arrange
            var command = new AddedAccountProviderCommand(invitation.Ukprn, Guid.NewGuid(), invitation.Reference.ToString(), DateTime.Now);
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedAccountProviderCommand>)handler).Handle(command, new CancellationToken());

            // assert
            (await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id)).Should().BeNull();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvitationDoesNotExist_ThenErrorIsThrown(
            ProviderRegistrationsDbContext setupContext,
            AddedAccountProviderCommandHandler handler,
            [Greedy]Invitation invitation)
        {
            //arrange            
            var command = new AddedAccountProviderCommand(12345, Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            try
            {
                await ((IRequestHandler<AddedAccountProviderCommand>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation ID found for CorrelationId:{command.CorrelationId}");
            }
        }
    }
}