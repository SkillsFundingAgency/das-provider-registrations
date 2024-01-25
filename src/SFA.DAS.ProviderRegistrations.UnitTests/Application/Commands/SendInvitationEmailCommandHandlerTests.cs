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
using Microsoft.Extensions.Logging;
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
            [Frozen] ProviderRegistrationsSettings settings,
            SendInvitationEmailCommand command)
        {

            //arrange
            settings.UseGovLogin = false;
            var tokens = new Dictionary<string, string>
            {
                { "provider_organisation", command.ProviderOrgName },
                { "provider_name", command.ProviderUserFullName },
                { "employer_organisation", command.EmployerOrganisation },
                { "employer_name", command.EmployerFullName },
                { "invitation_link", $"{settings.EmployerAccountsBaseUrl}/service/register/{command.CorrelationId}" },
                { "unsubscribe_training_provider", $"{settings.EmployerAccountsBaseUrl}/service/unsubscribe/{command.CorrelationId}" },
                { "report_training_provider", $"{settings.EmployerAccountsBaseUrl}/report/trainingprovider/{command.CorrelationId}" }
            };
            var handler = new SendInvitationEmailCommandHandler(mockPublisher.Object, settings, Mock.Of<ILogger<SendInvitationEmailCommandHandler>>());

            //act
            await ((IRequestHandler<SendInvitationEmailCommand>)handler).Handle(command, new CancellationToken());

            //assert
            mockPublisher.Verify(s => s.Send(It.Is<SendEmailCommand>(t =>
                t.TemplateId == "ProviderInviteEmployerNotification" &&
                t.RecipientsAddress == command.EmployerEmail &&
                t.Tokens.OrderBy(kvp => kvp.Key).SequenceEqual(tokens.OrderBy(kvp => kvp.Key))), It.IsAny<SendOptions>()));
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingSendInvitationEmailCommand_ThenShouldAddInvitationForGovProd(
            [Frozen] Mock<IMessageSession> mockPublisher,
            [Frozen] ProviderRegistrationsSettings settings,
            SendInvitationEmailCommand command)
        {

            //arrange
            settings.ResourceEnvironmentName = "PRD";
            settings.UseGovLogin = true;
            var tokens = new Dictionary<string, string>
            {
                { "provider_organisation", command.ProviderOrgName },
                { "provider_name", command.ProviderUserFullName },
                { "employer_organisation", command.EmployerOrganisation },
                { "employer_name", command.EmployerFullName },
                { "invitation_link", $"{settings.EmployerAccountsBaseUrl}/service/register/{command.CorrelationId}" },
                { "unsubscribe_training_provider", $"{settings.EmployerAccountsBaseUrl}/service/unsubscribe/{command.CorrelationId}" },
                { "report_training_provider", $"{settings.EmployerAccountsBaseUrl}/report/trainingprovider/{command.CorrelationId}" }
            };
            var handler = new SendInvitationEmailCommandHandler(mockPublisher.Object, settings, Mock.Of<ILogger<SendInvitationEmailCommandHandler>>());

            //act
            await ((IRequestHandler<SendInvitationEmailCommand>)handler).Handle(command, new CancellationToken());

            //assert
            mockPublisher.Verify(s => s.Send(It.Is<SendEmailCommand>(t =>
                t.TemplateId == "9dc52d84-0ee5-4755-b836-f4e71ae2a326" &&
                t.RecipientsAddress == command.EmployerEmail &&
                t.Tokens.OrderBy(kvp => kvp.Key).SequenceEqual(tokens.OrderBy(kvp => kvp.Key))), It.IsAny<SendOptions>()));
        }

        [Test, ProviderAutoData]
        public async Task Handle_WhenHandlingSendInvitationEmailCommand_ThenShouldAddInvitationForGovNonProd(
            [Frozen] Mock<IMessageSession> mockPublisher,
            [Frozen] ProviderRegistrationsSettings settings,
            SendInvitationEmailCommand command)
        {

            //arrange
            settings.ResourceEnvironmentName = "TesT";
            settings.UseGovLogin = true;
            var tokens = new Dictionary<string, string>
            {
                { "provider_organisation", command.ProviderOrgName },
                { "provider_name", command.ProviderUserFullName },
                { "employer_organisation", command.EmployerOrganisation },
                { "employer_name", command.EmployerFullName },
                { "invitation_link", $"{settings.EmployerAccountsBaseUrl}/service/register/{command.CorrelationId}" },
                { "unsubscribe_training_provider", $"{settings.EmployerAccountsBaseUrl}/service/unsubscribe/{command.CorrelationId}" },
                { "report_training_provider", $"{settings.EmployerAccountsBaseUrl}/report/trainingprovider/{command.CorrelationId}" }
            };

            var handler = new SendInvitationEmailCommandHandler(mockPublisher.Object, settings, Mock.Of<ILogger<SendInvitationEmailCommandHandler>>());

            //act
            await ((IRequestHandler<SendInvitationEmailCommand>)handler).Handle(command, new CancellationToken());

            //assert
            mockPublisher.Verify(s => s.Send(It.Is<SendEmailCommand>(t =>
                t.TemplateId == "02818d7b-cea1-4445-8b16-5a27f40ddaf6" &&
                t.RecipientsAddress == command.EmployerEmail &&
                t.Tokens.OrderBy(kvp => kvp.Key).SequenceEqual(tokens.OrderBy(kvp => kvp.Key))), It.IsAny<SendOptions>()));
        }
    }
}