﻿using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Controllers.RegistrationControllerUnitTests
{
    [TestFixture]
    public class WhenIPostNewEmployerUserViewModel
    {
        [Test, DomainAutoData]
        public async Task ThenAnInvitationIsAdded(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IAuthenticationService> authService,
            RegistrationController controller,
            NewEmployerUserViewModel model,
            string command,
            string providerName)
        {
            //arrange
            authService.Object.TryGetUserClaimValue(ProviderClaims.DisplayName, out var displayName);
            mediator.Setup(s => s.Send(It.IsAny<GetProviderByUkprnQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetProviderByUkprnQueryResult(providerName));

            //act
            await controller.InviteEmployeruser(model, command);

            //assert
            mediator.Verify(x => x.Send(It.Is<AddInvitationCommand>(s => 
            s.EmployerEmail == model.EmployerEmailAddress && 
            s.EmployerFirstName == model.EmployerFirstName &&
            s.EmployerLastName == model.EmployerLastName &&
            s.EmployerOrganisation == model.EmployerOrganisation &&
            s.Ukprn == authService.Object.Ukprn.Value &&
            s.UserRef == authService.Object.UserId &&
            s.ProviderOrganisationName == providerName &&
            s.ProviderUserFullName == displayName), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
