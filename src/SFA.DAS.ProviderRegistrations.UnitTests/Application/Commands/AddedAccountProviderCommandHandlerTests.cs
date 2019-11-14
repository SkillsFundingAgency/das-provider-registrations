using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddedAccountProviderCommandTests : FluentTest<AddedAccountProviderCommandHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Invitation.Status.Should().Be((int) InvitationStatus.InvitationComplete);
            });
        }

        [Test]
        public Task Handle_WhenDoesntExistCommandIsHandled_ThenNoChangesAreMade()
        {
            return RunAsync(f => f.HandleDoesntExist(), f =>
            {
                f.Invitation.Status.Should().Be((int) InvitationStatus.InvitationSent);
                f.InvitationDoesntExist.Status.Should().Be((int) InvitationStatus.PayeSchemeAdded);
                f.InvitationInvalidStatus.Status.Should().Be((int) InvitationStatus.InvitationComplete);
            });
        }

        [Test]
        public Task Handle_WhenInvalidStatusCommandIsHandled_ThenNoChangesAreMade()
        {
            return RunAsync(f => f.HandleInvalidStatus(), f =>
            {
                f.InvitationInvalidStatus.Status.Should().Be((int) InvitationStatus.InvitationComplete);
            });
        }
    }

    public class AddedAccountProviderCommandHandlerTestFixture
    {
        public ProviderRegistrationsDbContext Db { get; set; }
        public Invitation Invitation { get; set; }
        public Invitation InvitationDoesntExist { get; set; }
        public Invitation InvitationInvalidStatus { get; set; }
        public AddedAccountProviderCommand Command { get; set; }
        public AddedAccountProviderCommand CommandDoesntExist { get; set; }
        public AddedAccountProviderCommand CommandInvalidStatus { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public IRequestHandler<AddedAccountProviderCommand, Unit> Handler { get; set; }
        
        public AddedAccountProviderCommandHandlerTestFixture()
        {
            Guid correlationId1 = Guid.NewGuid();
            Guid correlationId2 = Guid.NewGuid();
            Guid correlationId3 = Guid.NewGuid();
            Db = new ProviderRegistrationsDbContext(new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new AddedAccountProviderCommand(12345, Guid.NewGuid(), correlationId1.ToString());
            CommandDoesntExist = new AddedAccountProviderCommand(12345, Guid.NewGuid(), Guid.NewGuid().ToString());
            CommandInvalidStatus = new AddedAccountProviderCommand(12345, Guid.NewGuid(), correlationId3.ToString());

            Invitation = new Invitation(correlationId1, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.InvitationSent, DateTime.Now, DateTime.Now);
            InvitationDoesntExist = new Invitation(correlationId2, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.PayeSchemeAdded, DateTime.Now, DateTime.Now);
            InvitationInvalidStatus = new Invitation(correlationId3, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.InvitationComplete, DateTime.Now, DateTime.Now);

            Db.Invitations.Add(Invitation);
            Db.Invitations.Add(InvitationDoesntExist);
            Db.Invitations.Add(InvitationInvalidStatus);
            Db.SaveChanges();

            Handler = new AddedAccountProviderCommandHandler(new Lazy<ProviderRegistrationsDbContext>(() => Db));
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