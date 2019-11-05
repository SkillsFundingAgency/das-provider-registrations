using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.Controllers;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.InvitationsControllerUnitTests
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnInvitationFromQuery()
        {
            return RunAsync(
                f => f.CallGet(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkObjectResult>();

                    var okObjectResult = r as OkObjectResult;
                    okObjectResult.Should().NotBeNull();
                   
                    var model = okObjectResult?.Value as InvitationDto;
                    model.Should().NotBeNull();
                    model.Should().BeEquivalentTo(f.Result.Invitation);
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
        public InvitationsController InvitationsController { get; set; }
        public GetInvitationByIdQueryResult Result { get; set; }

        public GetTestsFixture()
        {
            CorrelationId = Guid.NewGuid().ToString();

            Mediator = new Mock<IMediator>();

            Result = new GetInvitationByIdQueryResult(new InvitationDto());

            Mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == Guid.Parse(CorrelationId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result);

            InvitationsController = new InvitationsController(Mediator.Object);
        }

        public async Task<ActionResult> CallGet()
        {
            return await InvitationsController.Get(CorrelationId, CancellationToken.None);
        }

        public GetTestsFixture SetCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }
    }
}