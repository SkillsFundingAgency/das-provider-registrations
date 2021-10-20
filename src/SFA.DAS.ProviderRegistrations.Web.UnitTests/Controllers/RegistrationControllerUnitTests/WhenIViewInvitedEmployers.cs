using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Types;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIViewInvitedEmployers
    {
        [Test, DomainAutoData]
        public async Task ThenAnInvitationIsAdded(
            [Frozen] Mock<IAuthenticationService> authService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegistrationController controller)
        {
            //arrange
            mediator.Setup(x => x.Send(It.Is<GetInvitationQuery>(s => s.Ukprn == authService.Object.Ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetInvitationQueryResult(new List<InvitationDto>()));

            //act
            await controller.InvitedEmployers(null, null);

            //assert
            mediator.Verify(x => x.Send(It.IsAny<GetInvitationQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
