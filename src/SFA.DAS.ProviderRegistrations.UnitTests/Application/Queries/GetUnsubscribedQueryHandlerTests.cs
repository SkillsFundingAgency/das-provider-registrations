using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetUnsubscribedQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetUnsubscribedQueryAndParametersAreMatched_ThenShouldReturnGetATrueResult(
            [Frozen] ProviderRegistrationsDbContext db,
            Unsubscribe entity,
            GetUnsubscribedQueryHandler handler)
        {
            //arrange
            db.Unsubscribed.Add(entity);
            await db.SaveChangesAsync();
            var query = new GetUnsubscribedQuery(entity.Ukprn, entity.EmailAddress);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeTrue();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetUnsubscribedQueryAndParametersAreNotMatched_ThenShouldReturnAFalseResult(
            [Frozen] ProviderRegistrationsDbContext db,
            GetUnsubscribedQuery query,
            GetUnsubscribedQueryHandler handler)
        {
            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeFalse();
        }
    }
}