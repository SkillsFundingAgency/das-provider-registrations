using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationEventByIdQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationEventByIdQueryAndInvitationEventIsNotFound_ThenShouldReturnNull(
           GetInvitationEventByIdQueryHandler handler,
           GetInvitationEventByIdQuery query)
        {
            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().BeNull();
        }


        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationEventByIdQueryAndInvitationEventIsFound_ThenShouldReturnGetInvitationEventByIdQueryResult(
            ProviderRegistrationsDbContext setupContext,
            InvitationEvents invitationEvent,
            Invitation invitation,
            GetInvitationEventByIdQueryHandler handler)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            invitationEvent.InvitationId = invitation.Id;
            setupContext.InvitationEvents.Add(invitationEvent);
            await setupContext.SaveChangesAsync();
            var query = new GetInvitationEventByIdQuery(invitation.Id);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.InvitationEvent.Should().NotBeNull();
            result.InvitationEvent.Should().BeEquivalentTo(new
            {
                invitationEvent.AccountCreationStartedDate,
                invitationEvent.PayeSchemeAddedDate,
                invitationEvent.InvitationReSentDate,
                invitationEvent.AgreementAcceptedDate,
                invitation.EmployerOrganisation
            });
        }
    }
}
