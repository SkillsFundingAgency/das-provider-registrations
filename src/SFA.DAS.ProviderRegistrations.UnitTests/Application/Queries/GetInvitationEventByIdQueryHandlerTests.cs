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
            InvitationEvent invitationEvent1,
            InvitationEvent invitationEvent2,
            Invitation invitation,
            GetInvitationEventByIdQueryHandler handler)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            invitationEvent1.InvitationId = invitation.Id;
            invitationEvent1.EventType = 1;
            setupContext.InvitationEvents.Add(invitationEvent1);
            invitationEvent2.InvitationId = invitation.Id;
            invitationEvent2.EventType = 2;
            setupContext.InvitationEvents.Add(invitationEvent2);
            await setupContext.SaveChangesAsync();
            var query = new GetInvitationEventByIdQuery(invitation.Id);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.InvitationEvents.Should().NotBeNull();
            result.InvitationEvents.Count.Should().Be(2);
        }
    }
}
