using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationQueryHandlerTests : FluentTest<GetInvitationQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetInvitationQueryAndProviderIsFound_ThenShouldReturnGetInvitationQueryResult()
        {
            return RunAsync(f => f.SetInvitations(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetInvitationQueryResult>(
                    a => a.Invitations.First().Ukprn == 12345
                )
            );
        }
    }

    public class GetInvitationQueryHandlerTestsFixture
    {
        public GetInvitationQuery Query { get; set; }
        public IRequestHandler<GetInvitationQuery, GetInvitationQueryResult> Handler { get; set; }
        public Invitation Invitation { get; set; }
        public ProviderRegistrationsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        
        public GetInvitationQueryHandlerTestsFixture()
        {
            Query = new GetInvitationQuery(12345, null, "EmployerOrganisation", "Desc");
            Db = new ProviderRegistrationsDbContext(new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfile(typeof(InvitationMappings)));
            Handler = new GetInvitationQueryHandler(new Lazy<ProviderRegistrationsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetInvitationQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetInvitationQueryHandlerTestsFixture SetInvitations()
        {
            Invitation = EntityActivator.CreateInstance<Invitation>().Set(i => i.Ukprn, 12345).Set(i => i.EmployerOrganisation, "Org");
            Db.Invitations.Add(Invitation);
            Db.SaveChanges();
            
            return this;
        }
    }
}