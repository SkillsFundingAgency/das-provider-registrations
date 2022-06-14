using AutoFixture;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIViewEmployerAccountStatus
    {
        private Fixture fixture { get; set; }
        private GetInvitationEventByIdQueryResult queryResult { get; set; }

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            queryResult = fixture.Create<GetInvitationEventByIdQueryResult>();
        }

        [Test, DomainAutoData]
        public async Task ThenIViewStatus(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] RegistrationController controller,           
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
