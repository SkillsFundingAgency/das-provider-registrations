using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;

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
                AddedPayeSchemeCommand commandDetails,
                [Greedy]Invitation invitation)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now);
            var command = GetAddedPayeSchemeCommand(commandDetails, invitation.Reference.ToString());
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand>)handler).Handle(command, new CancellationToken());

            //assert
            var savedInvitation = await confirmationContext.Invitations.FirstAsync();
            savedInvitation.Status.Should().Be((int)InvitationStatus.PayeSchemeAdded);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
                ProviderRegistrationsDbContext setupContext,
                ProviderRegistrationsDbContext confirmationContext,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand commandDetails,
                [Greedy]Invitation invitation)
        {
            //arrange            
            var updatedDate = DateTime.Now;
            invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now.AddHours(-1));
            var command = GetAddedPayeSchemeCommand(commandDetails, invitation.Reference.ToString(), updatedDate);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id && s.EventType == (int)EventType.PayeSchemeAdded);
            addedInvitationEvent.Date.Should().Be(updatedDate);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
                ProviderRegistrationsDbContext setupContext,
                ProviderRegistrationsDbContext confirmationContext,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand commandDetails,
                [Greedy]Invitation invitation)
        {
            invitation.InvitationEvents.Clear();
            
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = GetAddedPayeSchemeCommand(commandDetails, invitation.Reference.ToString());

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand>)handler).Handle(command, new CancellationToken());

            // assert
            (await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id)).Should().BeNull();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvitationDoesNotExist_ThenErrorIsThrown(
                ProviderRegistrationsDbContext setupContext,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand commandDetails,
                [Greedy]Invitation invitation)
        {
            //arrange            
            invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            var differentInvitationCorrelationId = Guid.NewGuid();
            var command = GetAddedPayeSchemeCommand(commandDetails, differentInvitationCorrelationId.ToString());

            //act
            try
            {
                await ((IRequestHandler<AddedPayeSchemeCommand>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation ID found for CorrelationId:{command.CorrelationId}");
            }
        }

        private AddedPayeSchemeCommand GetAddedPayeSchemeCommand(AddedPayeSchemeCommand details, string correlationId, DateTime? eventDate = null)
        {
            return new AddedPayeSchemeCommand(
                details.AccountId,
                details.UserName,
                details.UserRef,
                details.PayeRef,
                details.Aorn,
                details.SchemeName,
                correlationId,
                eventDate.HasValue ? eventDate.Value : details.EventDateTime);
        }
    }
}
