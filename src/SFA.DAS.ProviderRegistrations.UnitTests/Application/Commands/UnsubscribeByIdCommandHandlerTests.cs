using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UnsubscribeByIdCommandHandlerTests : FluentTest<UnsubscribeByIdCommandHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Db.Unsubscribed.SingleOrDefault(u => u.Ukprn == 12345 && u.EmailAddress == "Email").Should().NotBeNull();
            });
        }
    }

    public class UnsubscribeByIdCommandHandlerTestFixture
    {
        public ProviderRegistrationsDbContext Db { get; set; }
        public Invitation Invitation { get; set; }
        public UnsubscribeByIdCommand Command { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public IRequestHandler<UnsubscribeByIdCommand, Unit> Handler { get; set; }
        
        public UnsubscribeByIdCommandHandlerTestFixture()
        {
            Guid correlationId = Guid.NewGuid();
           
            Db = new ProviderRegistrationsDbContext(new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new UnsubscribeByIdCommand(correlationId);
            
            Invitation = new Invitation(correlationId, 12345, "Ref", "Org", "FirstName", "LastName", "Email", (int) InvitationStatus.InvitationSent, DateTime.Now, DateTime.Now);
           
            Db.Invitations.Add(Invitation); 
            Db.SaveChanges();

            Handler = new UnsubscribeByIdCommandHandler(new Lazy<ProviderRegistrationsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}