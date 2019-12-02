using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetEmailAddressInUseQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetEmailAddressInUseQueryHandlerAndParametersAreMatched_ThenShouldReturnATrueResult(
            string emailAddress,
            GetEmailAddressInUseQueryHandler handler)
        {
            //arrange
            var query = new GetEmailAddressInUseQuery(emailAddress);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeTrue();
        }
    }
}