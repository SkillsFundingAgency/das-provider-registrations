using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Filters;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Filters
{
    class WhenAddingGoogleAnalyticsInformation
    {
        [Test, DomainAutoData]
        public async Task ThenProviderIdAndUserIdIsAddedToViewBag(
            uint ukPrn,
            string userId,
            [ArrangeActionContext] ActionExecutingContext context,
            [Frozen] Mock<ActionExecutionDelegate> nextMethod,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            var prnClaim = new Claim(ProviderClaims.Ukprn, ukPrn.ToString());
            var userClaim = new Claim(ProviderClaims.Upn, userId.ToString());
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { prnClaim, userClaim }));

            //Act
            await filter.OnActionExecutionAsync(context, nextMethod.Object);

            //Assert
            var actualController = context.Controller as Controller;
            Assert.IsNotNull(actualController);
            var viewBagData = actualController.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBagData);
            Assert.AreEqual(ukPrn.ToString(), viewBagData.UkPrn);
            Assert.AreEqual(userId, viewBagData.UserId);
        }

        [Test, DomainAutoData]
        public async Task AndContextIsNonController_ThenNoDataIsAddedToViewbag(
            long ukPrn,
            string userId,
            [ArrangeActionContext] ActionExecutingContext context,
            [Frozen] Mock<ActionExecutionDelegate> nextMethod,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            var prnClaim = new Claim(ProviderClaims.Ukprn, ukPrn.ToString());
            var userClaim = new Claim(ProviderClaims.Upn, userId);
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { prnClaim, userClaim }));

            var contextWithoutController = new ActionExecutingContext(
                new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor),
                context.Filters,
                context.ActionArguments,
                "");

            //Act
            await filter.OnActionExecutionAsync(contextWithoutController, nextMethod.Object);

            //Assert
            Assert.DoesNotThrowAsync(() => filter.OnActionExecutionAsync(contextWithoutController, nextMethod.Object));
        }
    }
}
