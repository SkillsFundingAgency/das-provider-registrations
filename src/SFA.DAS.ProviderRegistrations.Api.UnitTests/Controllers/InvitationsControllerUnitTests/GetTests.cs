using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.Controllers;
using SFA.DAS.ProviderRegistrations.Api.UnitTests.AutoFixture;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.InvitationsControllerUnitTests;

[TestFixture]
public class GetTests
{
    private Fixture _fixture;
    private GetInvitationByIdQueryResult _queryResult;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _queryResult = _fixture.Create<GetInvitationByIdQueryResult>();
    }

    [Test, DomainAutoData]
    public async Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnInvitationFromQuery(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] InvitationsController controller,            
        Guid correlationId)
    {
        //arrange
        mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == correlationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_queryResult);
            
        //act
        var result = await controller.Get(correlationId.ToString(), new CancellationToken());

        //assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<InvitationDto>()
            .Which.Should().BeEquivalentTo(_queryResult.Invitation);
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