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

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.Controllers.InvitationsControllerUnitTests
{
    [TestFixture]
    [Parallelizable]
    public class GetTests
    {
        [Test, DomainAutoData]
        public async Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnInvitationFromQuery(
            [Frozen] Mock<IMediator> mediator,
            InvitationsController controller,
            GetInvitationByIdQueryResult queryResult,
            Guid correlationId)
        {
            //arrange
            mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == correlationId), It.IsAny<CancellationToken>()))
             .ReturnsAsync(queryResult);
            
            //act
            var result = await controller.Get(correlationId.ToString(), new CancellationToken());

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<InvitationDto>()
                .Which.Should().BeEquivalentTo(queryResult.Invitation);
        }

        [Test, DomainAutoData]
        public async Task WhenCorrelationIdIsInvalid_ThenShouldReturnBadRequest(InvitationsController controller)
        {

            //act
            var result = await controller.Get("INVALID", new CancellationToken());

            //assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        }
    }
}