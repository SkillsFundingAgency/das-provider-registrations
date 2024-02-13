using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Exceptions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands;

[TestFixture]
[Parallelizable]
public class SignedAgreementCommandHandlerTests
{
    [Test, ProviderAutoData]
    public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
        ProviderRegistrationsDbContext setupContext,
        ProviderRegistrationsDbContext confirmationContext,
        SignedAgreementCommandHandler handler,
        SignedAgreementCommand commandDetails,
        [Greedy]Invitation invitation)
    {
        //arrange            
        var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString());
        setupContext.Invitations.Add(invitation);
        invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now);
        await setupContext.SaveChangesAsync();

        //act
        await ((IRequestHandler<SignedAgreementCommand>)handler).Handle(command, new CancellationToken());

        //assert
        var savedInvite = await confirmationContext.Invitations.FirstAsync(i => i.Reference.ToString() == command.CorrelationId);
        savedInvite.Status.Should().Be((int)InvitationStatus.LegalAgreementSigned);
    }

    [Test, ProviderAutoData]
    public async Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade(
        ProviderRegistrationsDbContext db,
        ProviderRegistrationsDbContext confirmationContext,
        SignedAgreementCommandHandler handler,
        SignedAgreementCommand commandDetails,
        [Greedy]Invitation invitation)
    {
        //arrange            
        var command = GetSignedAgreementCommand(commandDetails, string.Empty);
        db.Invitations.Add(invitation);
        invitation.UpdateStatus((int)InvitationStatus.InvitationSent, DateTime.Now);
        await db.SaveChangesAsync();

        //act
        await ((IRequestHandler<SignedAgreementCommand>)handler).Handle(command, new CancellationToken());

        //assert
        var savedInvite = await confirmationContext.Invitations.FirstAsync(i => i.Reference == invitation.Reference);
        savedInvite.Status.Should().Be((int)InvitationStatus.InvitationSent);
    }      

    [Test, ProviderAutoData]
    public async Task Handle_WhenCommandIsHandled_ThenShouldAddInvitationEvent(
        ProviderRegistrationsDbContext setupContext,
        ProviderRegistrationsDbContext confirmationContext,
        SignedAgreementCommandHandler handler,
        SignedAgreementCommand commandDetails,
        [Greedy]Invitation invitation)
    {
        //arrange
        var updateDate = DateTime.Now;
        var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString(), updateDate);
        setupContext.Invitations.Add(invitation);
        invitation.UpdateStatus((int)InvitationStatus.AccountStarted, DateTime.Now.AddHours(-1));
        await setupContext.SaveChangesAsync();

        //act
        await ((IRequestHandler<SignedAgreementCommand>)handler).Handle(command, new CancellationToken());

        //assert            
        var addedInvitationEvent = await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id && s.EventType == (int)EventType.LegalAgreementSigned);
        addedInvitationEvent.Date.Should().Be(updateDate);
    }

    [Test, ProviderAutoData]
    public async Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade(
        ProviderRegistrationsDbContext setupContext,
        ProviderRegistrationsDbContext confirmationContext,
        SignedAgreementCommandHandler handler,
        SignedAgreementCommand commandDetails,
        [Greedy]Invitation invitation)
    {
        invitation.InvitationEvents.Clear();
            
        //arrange            
        var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString());
        setupContext.Invitations.Add(invitation);
        invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
        await setupContext.SaveChangesAsync();

        //act
        await ((IRequestHandler<SignedAgreementCommand>)handler).Handle(command, new CancellationToken());

        // assert
        (await confirmationContext.InvitationEvents.FirstOrDefaultAsync(s => s.Invitation.Id == invitation.Id)).Should().BeNull();
    }

    [Test, ProviderAutoData]
    public async Task Handle_WhenInvitationDoesNotExist_ThenErrorIsThrown(
        ProviderRegistrationsDbContext setupContext,
        SignedAgreementCommandHandler handler,
        SignedAgreementCommand commandDetails,
        [Greedy]Invitation invitation)
    {
        //arrange            
        invitation.UpdateStatus((int)InvitationStatus.InvitationComplete, DateTime.Now);
        setupContext.Invitations.Add(invitation);
        await setupContext.SaveChangesAsync();

        var command = GetSignedAgreementCommand(commandDetails, invitation.Reference.ToString());

        //act
        try
        {
            await ((IRequestHandler<SignedAgreementCommand>)handler).Handle(command, new CancellationToken());
        }
        catch (InvalidInvitationException ex)
        {
            //assert
            Assert.That($"No invitation ID found for CorrelationId:{command.CorrelationId}", Is.EqualTo(ex.Message));
        }
    }

    private SignedAgreementCommand GetSignedAgreementCommand(SignedAgreementCommand details, string correlationId, DateTime? eventDate = null)
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
}