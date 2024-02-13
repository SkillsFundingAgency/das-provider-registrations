using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetUnsubscribedQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetUnsubscribedQueryAndParametersAreMatched_ThenShouldReturnGetATrueResult(
            ProviderRegistrationsDbContext setupContext,
            [Greedy]Unsubscribe entity,
            GetUnsubscribedQueryHandler handler)
        {
            //arrange
            setupContext.Unsubscribed.Add(entity);
            await setupContext.SaveChangesAsync();
            var query = new GetUnsubscribedQuery(entity.Ukprn, entity.EmailAddress);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeTrue();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetUnsubscribedQueryAndParametersAreNotMatched_ThenShouldReturnAFalseResult(
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