﻿using AutoFixture;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIReviewEmployerDetails
    {
        private Fixture fixture { get; set; }
        private GetInvitationByIdQueryResult queryResult { get; set; }

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            queryResult = fixture.Create<GetInvitationByIdQueryResult>();
        }


        [Test, DomainAutoData]
        public async Task ThenIReviewDetails(            
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegistrationController controller,            
            Guid correlationId)
        {
            //arrange
            mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == correlationId), It.IsAny<CancellationToken>()))
           .ReturnsAsync(queryResult);

            //act
            var result = await controller.ReviewDetails(correlationId);

            //assert
            mediator.Verify(x => x.Send(It.IsAny<GetInvitationByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);            
        }


        [Test, DomainAutoData]
        public async Task ThenCheckForUnsubscribed(
           [Frozen] Mock<IAuthenticationService> authService,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] RegistrationController controller,           
           Guid correlationId)
        {
            //arrange
            mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == correlationId), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);
            mediator.Setup(m => m.Send(It.Is<GetUnsubscribedQuery>(q => q.Ukprn == authService.Object.Ukprn && q.EmailAddress == queryResult.Invitation.EmployerEmail), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<bool>());

            //act
            await controller.ReviewDetails(correlationId);

            //assert
            mediator.Verify(x => x.Send(It.IsAny<GetInvitationByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
