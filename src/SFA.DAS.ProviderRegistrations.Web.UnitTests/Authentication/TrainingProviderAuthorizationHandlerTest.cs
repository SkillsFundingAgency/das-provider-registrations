using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.Services;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Authorization;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Authentication
{
    public class TrainingProviderAuthorizationHandlerTest
    {
        [Test, DomainAutoData]
        public async Task Then_The_ProviderDetails_Is_Valid_And_Handler_Return_True(
            long ukprn,
            GetProviderSummaryResult apiResponse,
            [Frozen] Mock<ITrainingProviderApiClient> trainingProviderApiClient,
            [Frozen] Mock<IAuthenticationService> authenticationService,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var providerId = ukprn.ToString();
            apiResponse.CanAccessApprenticeshipService = true;
            authenticationService.Setup(x => x.IsUserAuthenticated()).Returns(true);
            authenticationService.Setup(x => x.TryGetUserClaimValue(ProviderClaims.Ukprn, out providerId)).Returns(true);
            trainingProviderApiClient.Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            Assert.That(actual, Is.True);
        }

        [Test, DomainAutoData]
        public async Task Then_The_ProviderDetails_Is_InValid_And_Handler_Return_False(
            long ukprn,
            GetProviderSummaryResult apiResponse,
            [Frozen] Mock<ITrainingProviderApiClient> trainingProviderApiClient,
            [Frozen] Mock<IAuthenticationService> authenticationService,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var providerId = ukprn.ToString();
            apiResponse.CanAccessApprenticeshipService = false;
            authenticationService.Setup(x => x.IsUserAuthenticated()).Returns(true);
            authenticationService.Setup(x => x.TryGetUserClaimValue(ProviderClaims.Ukprn, out providerId)).Returns(true);
            trainingProviderApiClient.Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            Assert.That(actual, Is.False);
        }

        [Test, DomainAutoData]
        public async Task Then_The_ProviderDetails_Is_Null_And_Handler_Return_False(
            long ukprn,
            GetProviderSummaryResult apiResponse,
            [Frozen] Mock<ITrainingProviderApiClient> trainingProviderApiClient,
            [Frozen] Mock<IAuthenticationService> authenticationService,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var providerId = ukprn.ToString();
            apiResponse.CanAccessApprenticeshipService = false;
            authenticationService.Setup(x => x.IsUserAuthenticated()).Returns(true);
            authenticationService.Setup(x => x.TryGetUserClaimValue(ProviderClaims.Ukprn, out providerId)).Returns(true);
            trainingProviderApiClient.Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync((GetProviderSummaryResult)null);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            Assert.That(actual, Is.False);
        }

        [Test, DomainAutoData]
        public async Task Then_The_UserAuthenticationService_Return_False_And_Handler_Return_False(
            [Frozen] Mock<IAuthenticationService> authenticationService,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            authenticationService.Setup(x => x.IsUserAuthenticated()).Returns(false);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            Assert.That(actual, Is.False);
        }
    }
}
