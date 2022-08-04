using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.Controllers;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Types;
using SFA.DAS.ProviderRegistrations.Api.UnitTests.AutoFixture;
using AutoFixture.NUnit3;
using AutoFixture;
using System.Linq;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.InvitationsControllerUnitTests
{
    [TestFixture]
    public class GetTests
    {
        private Fixture Fixture { get; set; }
        private GetInvitationByIdQueryResult queryResult { get; set; }

        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
            Fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            queryResult = Fixture.Create<GetInvitationByIdQueryResult>();
        }

        [Test, DomainAutoData]
        public async Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnInvitationFromQuery(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] InvitationsController controller,            
            Guid correlationId)
        {
            //arrange
            mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == correlationId), It.IsAny<CancellationToken>()))
             .ReturnsAsync(queryResult);
            
            //act
            var result = await controller.Get(correlationId.ToString(), new CancellationToken());

            //assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<InvitationDto>()
                .Which.Should().BeEquivalentTo(queryResult.Invitation);
        }

        [Test, DomainAutoData]
        public async Task WhenCorrelationIdIsInvalid_ThenShouldReturnBadRequest(
            [Greedy] InvitationsController controller)
        {

            //act
            var result = await controller.Get("INVALID", new CancellationToken());

            //assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}