using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationQueryHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationQueryAndProviderIsFound_ThenShouldReturnGetInvitationQueryResult(
            [Frozen] ProviderRegistrationsDbContext db,
            GetInvitationQueryHandler handler,
            Invitation invitation)
        {
            //arrange
            db.Invitations.Add(invitation);
            await db.SaveChangesAsync();
            var query = new GetInvitationQuery(invitation.Ukprn, invitation.UserRef, "EmployerOrganisation", "Desc");

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