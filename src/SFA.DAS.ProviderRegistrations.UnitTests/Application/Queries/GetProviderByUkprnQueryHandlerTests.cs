using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprn;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetProviderByUkprnQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetProviderByUkprnQueryAndParametersAreMatched_ThenShouldReturnGetAProviderNameResult(
            long ukprn,
            GetProviderByUkprnQueryHandler handler)
        {
            //arrange
            var query = new GetProviderByUkprnQuery(ukprn);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().NotBeNull();
            result.ProviderName.Should().NotBeNullOrWhiteSpace();
        }
    }
}