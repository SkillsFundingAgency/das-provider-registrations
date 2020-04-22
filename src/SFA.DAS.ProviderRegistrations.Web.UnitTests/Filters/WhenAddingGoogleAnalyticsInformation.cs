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
        public async Task ThenProviderIdIsAddedToViewBag(
            uint ukPrn,
            [ArrangeActionContext] ActionExecutingContext context,
            [Frozen] Mock<ActionExecutionDelegate> nextMethod,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            var claim = new Claim(ProviderClaims.Ukprn, ukPrn.ToString());
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));

            //Act
            await filter.OnActionExecutionAsync(context, nextMethod.Object);

            //Assert
            var actualController = context.Controller as Controller;
            Assert.IsNotNull(actualController);
            var viewBagData = actualController.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBagData);
            Assert.AreEqual(ukPrn.ToString(), viewBagData.UkPrn);
        }
    }
}
