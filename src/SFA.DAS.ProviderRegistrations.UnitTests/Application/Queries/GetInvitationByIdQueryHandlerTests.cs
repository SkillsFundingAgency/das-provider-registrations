﻿using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
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
    public class GetInvitationByIdQueryHandlerTests
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
        public async Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsNotFound_ThenShouldReturnNull(
            GetInvitationByIdQueryHandler handler,
            GetInvitationByIdQuery query)
        {
            //act
            var result = await handler.Handle(query, new CancellationToken());
            
            //assert
            result.Should().BeNull();
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsFound_ThenShouldReturnGetInvitationByIdQueryResult(
            ProviderRegistrationsDbContext setupContext,            
            GetInvitationByIdQueryHandler handler,
            [Greedy]Invitation invitation)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var query = new GetInvitationByIdQuery(invitation.Reference);

            //act
            var result = await handler.Handle(query, new CancellationToken());

            //assert
            result.Invitation.Should().NotBeNull();
            result.Invitation.Should().BeEquivalentTo(new
            {
                invitation.Ukprn,
                invitation.EmployerFirstName,
                invitation.EmployerLastName,
                invitation.EmployerEmail,
                invitation.EmployerOrganisation,
                invitation.Status,
                SentDate = invitation.UpdatedDate
            });
        }
    }
}