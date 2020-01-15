using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand;
using SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class SendInvitationEmailCommandHandlerTests
    {
        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingSendInvitationEmailCommand_ThenShouldAddInvitation(
            [Frozen] Mock<IMessageSession> mockPublisher,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] ProviderRegistrationsSettings settings,
            SendInvitationEmailCommandHandler handler,
            SendInvitationEmailCommand command,
            string providerName)
        {

            //arrange
            mediator.Setup(s => s.Send(It.Is<GetProviderByUkprnQuery>(r => r.Ukprn == command.Ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderByUkprnQueryResult(providerName));
            var tokens = new Dictionary<string, string>
            {
                { "provider_organisation", providerName },
                { "provider_name", command.ProviderFullName },
                { "employer_organisation", command.EmployerOrganisation },
                { "employer_name", command.EmployerFullName },
                { "invitation_link", $"{settings.EmployerAccountsBaseUrl}/service/register/{command.CorrelationId}" },
                { "unsubscribe_training_provider", $"{settings.EmployerAccountsBaseUrl}/service/unsubscribe/{command.CorrelationId}" }
            };

            //act
            var result = await ((IRequestHandler<SendInvitationEmailCommand, Unit>)handler).Handle(command, new CancellationToken());

            //assert
            mockPublisher.Verify(s => s.Send(It.Is<SendEmailCommand>(t =>
                t.TemplateId == "ProviderInviteEmployerNotification" &&
                t.RecipientsAddress == command.EmployerEmail &&
                t.Tokens.OrderBy(kvp => kvp.Key).SequenceEqual(tokens.OrderBy(kvp => kvp.Key))),It.IsAny<SendOptions>()));
        }
    }
}