using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class SignedAgreementCommandHandlerTests : FluentTest<SignedAgreementCommandHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Invitation.Status.Should().Be((int) InvitationStatus.LegalAgreementSigned);
            });
        }

        [Test]
        public Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade()
        {
            return RunAsync(f => f.HandleDoesntExist(), f =>
            {
                f.Invitation.Status.Should().Be((int) InvitationStatus.PayeSchemeAdded);
                f.InvitationDoesntExist.Status.Should().Be((int) InvitationStatus.PayeSchemeAdded);
                f.InvitationInvalidStatus.Status.Should().Be((int) InvitationStatus.LegalAgreementSigned);
            });
        }

        [Test]
        public Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade()
        {
            return RunAsync(f => f.HandleInvalidStatus(), f =>
            {
                f.InvitationInvalidStatus.Status.Should().Be((int) InvitationStatus.LegalAgreementSigned);
            });
        }
    }

    public class SignedAgreementCommandHandlerTestFixture
    {
        public ProviderRegistrationsDbContext Db { get; set; }
        public Invitation Invitation { get; set; }
        public Invitation InvitationDoesntExist { get; set; }
        public Invitation InvitationInvalidStatus { get; set; }
        public SignedAgreementCommand Command { get; set; }
        public SignedAgreementCommand CommandDoesntExist { get; set; }
        public SignedAgreementCommand CommandInvalidStatus { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public IRequestHandler<SignedAgreementCommand, Unit> Handler { get; set; }
        
        public SignedAgreementCommandHandlerTestFixture()
        {
            Guid correlationId1 = Guid.NewGuid();
            Guid correlationId2 = Guid.NewGuid();
            Guid correlationId3 = Guid.NewGuid();
            Db = new ProviderRegistrationsDbContext(new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new SignedAgreementCommand(1, 1, "Org", 1, true, "Name", Guid.NewGuid(), correlationId1.ToString());
            CommandDoesntExist = new SignedAgreementCommand(2, 2, "Org", 2, true, "Name", Guid.NewGuid(), Guid.NewGuid().ToString());
            CommandInvalidStatus = new SignedAgreementCommand(3, 3, "Org", 3, true, "Name", Guid.NewGuid(), correlationId3.ToString());

            Invitation = new Invitation(correlationId1, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.PayeSchemeAdded, DateTime.Now, DateTime.Now);
            InvitationDoesntExist = new Invitation(correlationId2, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.PayeSchemeAdded, DateTime.Now, DateTime.Now);
            InvitationInvalidStatus = new Invitation(correlationId3, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.LegalAgreementSigned, DateTime.Now, DateTime.Now);

            Db.Invitations.Add(Invitation);
            Db.Invitations.Add(InvitationDoesntExist);
            Db.Invitations.Add(InvitationInvalidStatus);
            Db.SaveChanges();

            Handler = new SignedAgreementCommandHandler(new Lazy<ProviderRegistrationsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public async Task HandleDoesntExist()
        {
            await Handler.Handle(CommandDoesntExist, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public async Task HandleInvalidStatus()
        {
            await Handler.Handle(CommandInvalidStatus, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}