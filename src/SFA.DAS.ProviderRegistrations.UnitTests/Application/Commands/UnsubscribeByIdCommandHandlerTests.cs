using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UnsubscribeByIdCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenCommandIsHandled_ThenShouldUpdateInvitationStatus(
           ProviderRegistrationsDbContext setupContext,
           ProviderRegistrationsDbContext confirmationContext,
           UnsubscribeByIdCommandHandler handler,
           Invitation invitation)
        {
            //arrange
            setupContext.Invitations.Add(invitation);
            await setupContext.SaveChangesAsync();
            var command = new UnsubscribeByIdCommand(invitation.Reference);

            //act
            await ((IRequestHandler<UnsubscribeByIdCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            var unsubscribe = confirmationContext.Unsubscribed.Single(s => s.EmailAddress == invitation.EmployerEmail && s.Ukprn == invitation.Ukprn);
            unsubscribe.Should().NotBeNull();
        }
    }
}