using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddedPayeSchemeCommandHandlerTests
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
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand command)
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
        public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
                ProviderRegistrationsDbContext setupContext,
                ProviderRegistrationsDbContext confirmationContext,
                AddedPayeSchemeCommandHandler handler,
                AddedPayeSchemeCommand command)
        {
            //arrange            
            invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.UtcNow);
            command.CorrelationId = invitation.Reference.ToString();
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();

            //act
            await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id && s.EventType == (int)EventType.PayeSchemeAdded);
            addedInvitationEvent.Date.Date.Should().Be(DateTime.UtcNow.Date);
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedPayeSchemeCommandHandler handler,
            AddedPayeSchemeCommand command)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var statusBefore = invitation.Status;
            command.CorrelationId = invitation.Reference.ToString();

            //act
            try
            {
                await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());
            }
            catch (Exception ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation ID found for CorrelationId:{command.CorrelationId}");
            }
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
            ProviderRegistrationsDbContext setupContext,
            ProviderRegistrationsDbContext confirmationContext,
            AddedPayeSchemeCommandHandler handler,
            AddedPayeSchemeCommand command)
        {
            //arrange
            invitation.UpdateStatus((int)InvitationStatus.LegalAgreementSigned, DateTime.Now);
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            command.CorrelationId = invitation.Reference.ToString();

            //act
            try
            {
                await ((IRequestHandler<AddedPayeSchemeCommand, Unit>)handler).Handle(command, new CancellationToken());
            }
            catch (InvalidInvitationException ex)
            {
                //assert
                Assert.AreEqual(ex.Message, $"No invitation ID found for CorrelationId:{command.CorrelationId}") ;
            }
        }
    }
}
