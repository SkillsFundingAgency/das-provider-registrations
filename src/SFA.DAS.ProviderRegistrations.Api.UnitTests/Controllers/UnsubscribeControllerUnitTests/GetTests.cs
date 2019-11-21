using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.Controllers;
using SFA.DAS.ProviderRegistrations.Api.UnitTests.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.UnsubscribeControllerUnitTests
{
    [TestFixture]
    [Parallelizable]
    public class GetTests
    {
        [Test, DomainAutoData]
        public async Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnOk(
            UnsubscribeController controller,
            Guid correlationId)
        {
            //arrange

            //act
            var result = await controller.Get(correlationId.ToString(), new CancellationToken());

            //assert
            result.Should().NotBeNull().And.BeOfType<OkResult>();
        }

        [Test, DomainAutoData]
        public async Task WhenCorrelationIdIsInvalid_ThenShouldReturnBadRequest(
            UnsubscribeController controller)
        {
            //arrange

            //act
            var result = await controller.Get("INVALID", new CancellationToken());

            //assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        }
    }
}