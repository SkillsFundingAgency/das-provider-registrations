using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationEventByIdQueryHandlerTests
    {
        private Fixture fixture { get; set; }

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationEventByIdQueryAndInvitationEventIsFound_ThenShouldReturnGetInvitationEventByIdQueryResult(
            ProviderRegistrationsDbContext setupContext,             
            GetInvitationEventByIdQueryHandler handler,
            [Greedy]Invitation invitation)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            invitation.InvitationEvents.Clear();
            var invitationEvent1 = fixture.Create<InvitationEvent>();
            invitationEvent1.Invitation = invitation;
            invitationEvent1.EventType = 1;
            setupContext.InvitationEvents.Add(invitationEvent1);
            var invitationEvent2 = fixture.Create<InvitationEvent>();
            invitationEvent2.Invitation = invitation;
            invitationEvent2.EventType = 2;
            setupContext.InvitationEvents.Add(invitationEvent2);
            await setupContext.SaveChangesAsync();
            var query = new GetInvitationEventByIdQuery(invitation.Id);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Invitation.Should().NotBeNull();
            result.Invitation.Events.Count().Should().Be(2);
        }
    }
}
