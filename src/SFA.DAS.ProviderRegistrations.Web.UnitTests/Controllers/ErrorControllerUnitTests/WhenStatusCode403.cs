using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;
using System;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.ErrorControllerUnitTests
{
    public class WhenStatusCode403
    {
        [Test, DomainInlineAutoData(false, "LOCAL")]
        [DomainInlineAutoData(true, "LOCAL")]
        [DomainInlineAutoData(false, "PRD")]
        [DomainInlineAutoData(true, "PRD")]
        public void Then_The_HelpLink_Url_Is_Passed_To_The_View(
            bool useDfESignIn,
            string env,
            [Frozen] Mock<IConfiguration> configuration,
            [Greedy] ErrorController errorController)
        {
            
            configuration?.SetupGet(x => x[It.Is<string>(s => s == "ResourceEnvironmentName")]).Returns(env);
            configuration?.Setup(x => x[It.Is<string>(s => s == "UseDfESignIn")]).Returns(Convert.ToString(useDfESignIn));

            var actual = errorController.Error(403) as ViewResult;

            Assert.IsNotNull(actual);
            var actualModel = actual.Model as Error403ViewModel;
            Assert.IsNotNull(actualModel);
            Assert.AreEqual(actualModel.UseDfESignIn, useDfESignIn);
        }
    }
}
