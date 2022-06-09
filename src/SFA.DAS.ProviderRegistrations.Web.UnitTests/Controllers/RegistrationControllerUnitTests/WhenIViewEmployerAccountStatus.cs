using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIViewEmployerAccountStatus
    {
        [Test, DomainAutoData]
        public async Task ThenIViewStatus(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] RegistrationController controller,
           GetInvitationEventByIdQueryResult queryResult,
           long invitationId)
        {
            //arrange
            mediator.Setup(m => m.Send(It.Is<GetInvitationEventByIdQuery>(q => q.InvitationId == invitationId), It.IsAny<CancellationToken>()))
           .ReturnsAsync(queryResult);

            //act
            var result = await controller.ViewStatus(invitationId);

            //assert
            mediator.Verify(x => x.Send(It.IsAny<GetInvitationEventByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test, DomainAutoData]
        public async Task ThenIViewStatusWithGetInvitationEventByIdQueryIsNull(
          [Frozen] Mock<IMediator> mediator,
          [Greedy] RegistrationController controller,
          GetInvitationEventByIdQueryResult queryResult,
          long invitationId)
        {
            //arrange
            queryResult = null;
            mediator.Setup(m => m.Send(It.Is<GetInvitationEventByIdQuery>(q => q.InvitationId == invitationId), It.IsAny<CancellationToken>()))
           .ReturnsAsync(queryResult);

            //act
            var result = await controller.ViewStatus(invitationId);

            //assert
            mediator.Verify(x => x.Send(It.IsAny<GetInvitationEventByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
