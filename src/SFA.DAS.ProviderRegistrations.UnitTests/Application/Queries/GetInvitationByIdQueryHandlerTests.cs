using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationByIdQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsNotFound_ThenShouldReturnNull(
            GetInvitationByIdQueryHandler handler,
            GetInvitationByIdQuery query)
        {
            var result = await handler.Handle(query, new CancellationToken());
            result.Should().BeNull();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsFound_ThenShouldReturnGetInvitationByIdQueryResult(
            [Frozen] ProviderRegistrationsDbContext db,
            Invitation invitation,
            GetInvitationByIdQueryHandler handler
            )
        {
            db.Invitations.Add(invitation);
            await db.SaveChangesAsync();

            var query = new GetInvitationByIdQuery(invitation.Reference);
            var result = await handler.Handle(query, new CancellationToken());

            result.Invitation.Should().NotBeNull();
            result.Invitation.Should().BeEquivalentTo(new
            {
                invitation.Ukprn,
                invitation.EmployerFirstName,
                invitation.EmployerLastName,
                invitation.EmployerEmail,
                invitation.EmployerOrganisation,
                invitation.Status,
                SentDate = invitation.CreatedDate
            });
        }
    }
}