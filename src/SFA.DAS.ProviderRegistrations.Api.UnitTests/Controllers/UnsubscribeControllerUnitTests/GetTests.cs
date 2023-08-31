using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.Controllers;
using SFA.DAS.ProviderRegistrations.Api.UnitTests.AutoFixture;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.UnsubscribeControllerUnitTests;

[TestFixture]
public class GetTests
{
    [Test, DomainAutoData]
    public async Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnOk(
        [Greedy] UnsubscribeController controller,
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
        [Greedy] UnsubscribeController controller)
    {
        //arrange

        //act
        var result = await controller.Get("INVALID", new CancellationToken());

        //assert
        result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
    }
}