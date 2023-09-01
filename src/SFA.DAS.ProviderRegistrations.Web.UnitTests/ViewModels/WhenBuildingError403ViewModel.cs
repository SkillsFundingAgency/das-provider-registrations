using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.ViewModels
{
    public class WhenBuildingError403ViewModel
    {
        [Test]
        public void Then_The_HelpPage_Link_Is_Correct_For_Production_Environment()
        {
            var actual = new Error403ViewModel("prd");

            Assert.That(actual.HelpPageLink, Is.Not.Null);
            Assert.AreEqual(actual.HelpPageLink, "https://services.signin.education.gov.uk/approvals/select-organisation?action=request-service");
        }

        [Test]
        public void Then_The_HelpPage_Link_Is_Correct_For_Empty_Environment()
        {
            var actual = new Error403ViewModel("");

            Assert.That(actual.HelpPageLink, Is.Not.Null);
            Assert.AreEqual(actual.HelpPageLink, "https://services.signin.education.gov.uk/approvals/select-organisation?action=request-service");
        }

        [Test]
        public void Then_The_HelpPage_Link_Is_Correct_For_Non_Production_Environment()
        {
            var actual = new Error403ViewModel("test");

            Assert.That(actual.HelpPageLink, Is.Not.Null);
            Assert.AreEqual(actual.HelpPageLink, "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service");
        }
    }
}
