using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddInvitationCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingAddInvitationCommand_ThenShouldAddInvitation(
            ProviderRegistrationsDbContext confirmationContext,
            AddInvitationCommandHandler handler,
            AddInvitationCommand command)
        {

            //arrange

            //act
            var result = await handler.Handle(command, new CancellationToken());

            //assert
            var invitation = confirmationContext.Invitations.First(f => f.Reference.ToString() == result);

            invitation.Should().BeEquivalentTo(new
            {
                command.Ukprn,
                command.UserRef,
                command.EmployerFirstName,
                command.EmployerLastName,
                command.EmployerEmail,
                command.EmployerOrganisation
            });
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingAddInvitationCommand_ThenShouldReturnReference(
            ProviderRegistrationsDbContext confirmationContext,
            AddInvitationCommandHandler handler,
            AddInvitationCommand command)
        {
            //arrange

            //act
            var result = await handler.Handle(command, new CancellationToken());

            //assert
            confirmationContext.Invitations.Should().ContainEquivalentOf(new { Reference = Guid.Parse(result) });
        }
    }
}