using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.Controllers;
using SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.UnsubscribeControllerUnitTests
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnOk()
        {
            return RunAsync(
                f => f.CallGet(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkResult>();

                    var okResult = r as OkResult;
                    okResult.Should().NotBeNull();
                });
        }

        [Test]
        public Task WhenCorrelationIdIsInvalid_ThenShouldReturnBadRequest()
        {
            return RunAsync(
                f => f.SetCorrelationId("INVALID"), 
                f => f.CallGet(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<BadRequestObjectResult>();

                    var badRequestObjectResult = r as BadRequestObjectResult;
                    badRequestObjectResult.Should().NotBeNull();
                });
        }
    }

    public class GetTestsFixture
    {
        public string CorrelationId { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public UnsubscribeController UnsubscribeController { get; set; }
     
        public GetTestsFixture()
        {
            CorrelationId = Guid.NewGuid().ToString();

            Mediator = new Mock<IMediator>();

            Mediator.Setup(m => m.Send(It.Is<UnsubscribeByIdCommand>(q => q.CorrelationId == Guid.Parse(CorrelationId)), It.IsAny<CancellationToken>()));

            UnsubscribeController = new UnsubscribeController(Mediator.Object);
        }

        public async Task<ActionResult> CallGet()
        {
            return await UnsubscribeController.Get(CorrelationId, CancellationToken.None);
        }

        public GetTestsFixture SetCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }
    }
}