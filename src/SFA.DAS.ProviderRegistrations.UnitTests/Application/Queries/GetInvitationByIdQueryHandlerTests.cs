using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationByIdQueryHandlerTests : FluentTest<GetInvitationByIdQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsFound_ThenShouldReturnGetInvitationByIdQueryResult()
        {
            return RunAsync(f => f.SetInvitation(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetInvitationByIdQueryResult>(r2 =>
                    r2.Invitation.EmployerOrganisation == f.Invitation.EmployerOrganisation &&
                    r2.Invitation.Ukprn == f.Invitation.Ukprn));
        }

        [Test]
        public Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetInvitationByIdQueryHandlerTestsFixture
    { 
        public GetInvitationByIdQuery Query { get; set; }
        public GetInvitationByIdQueryHandler Handler { get; set; }
        public Invitation Invitation { get; set; }
        public ProviderRegistrationsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetInvitationByIdQueryHandlerTestsFixture()
        {
            Query = new GetInvitationByIdQuery(Guid.NewGuid());
            Db = new ProviderRegistrationsDbContext(new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfile(typeof(InvitationMappings)));
            Handler = new GetInvitationByIdQueryHandler(new Lazy<ProviderRegistrationsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetInvitationByIdQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetInvitationByIdQueryHandlerTestsFixture SetInvitation()
        {
            Invitation = EntityActivator.CreateInstance<Invitation>().Set(p => p.Reference, Query.CorrelationId).Set(p => p.EmployerOrganisation, "Foo").Set(p => p.Ukprn, 12345);
            
            Db.Invitations.Add(Invitation);
            Db.SaveChanges();

            return this;
        }
    }
}