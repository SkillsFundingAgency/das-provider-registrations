using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationQueryAndProviderIsFound_ThenShouldReturnGetInvitationQueryResult(
            ProviderRegistrationsDbContext setupContext,
            GetInvitationQueryHandler handler,
            Invitation invitation)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var query = new GetInvitationQuery(invitation.Ukprn, invitation.UserRef, "EmployerOrganisation", "Desc", "EmployerFirstname");

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Should().NotBeNull();
            result.Invitations.Should().NotBeNull();
            result.Invitations.Count.Should().Be(1);
            result.Invitations.First().Should().BeEquivalentTo(
                new
                {
                    invitation.Ukprn,
                    invitation.Status,
                    invitation.EmployerOrganisation,
                    invitation.EmployerFirstName,
                    invitation.EmployerLastName,
                    invitation.EmployerEmail
                }
            );
        }
    }
}