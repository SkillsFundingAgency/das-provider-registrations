using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;
using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetEmailAddressInUseQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetEmailAddressInUseQueryHandlerAndParametersAreMatched_ThenShouldReturnATrueResult(
            string emailAddress,
            [Frozen] Mock<IEmployerUsersService> employerUserService,
            GetEmailAddressInUseQueryHandler handler)
        {
            //arrange
            var query = new GetEmailAddressInUseQuery(emailAddress);
            employerUserService.Setup(s => s.IsEmailAddressInUse(emailAddress, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeTrue();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetEmailAddressInUseQueryHandlerAndParametersAreNotMatched_ThenShouldReturnAFalseResult(
            string emailAddress,
            [Frozen] Mock<IEmployerUsersService> employerUserService,
            GetEmailAddressInUseQueryHandler handler)
        {
            //arrange
            var query = new GetEmailAddressInUseQuery(emailAddress);
            employerUserService.Setup(s => s.IsEmailAddressInUse(emailAddress, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeFalse();
        }
    }
}