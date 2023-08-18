using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.HomeControllerUnitTests
{
    public class WhenGettingTheDashboard
    {
        [Test, DomainAutoData]
        public void Then_The_Redirect(
            string dashboardUrl,
            [Frozen] ProviderSharedUIConfiguration config,
            [Greedy] HomeController controller)
        {
            config.DashboardUrl = dashboardUrl;

            var actual = controller.Dashboard() as RedirectResult;

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Url, dashboardUrl);
        }
    }
}
