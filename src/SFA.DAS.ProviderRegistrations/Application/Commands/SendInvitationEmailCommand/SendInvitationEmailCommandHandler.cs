using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand
{
    public class SendInvitationEmailCommandHandler : AsyncRequestHandler<SendInvitationEmailCommand>
    {
        private const string NotificationTemplateId = "ProviderInviteEmployerNotification";
        private readonly IMessageSession _publisher;
        private readonly IMediator _mediator;
        private readonly ProviderRegistrationsSettings _configuration;

        public SendInvitationEmailCommandHandler(IMediator mediator, IMessageSession publisher, ProviderRegistrationsSettings configuration)
        {
            _publisher = publisher;
            _mediator = mediator;
            _configuration = configuration;
        }

        protected override async Task Handle(SendInvitationEmailCommand request, CancellationToken cancellationToken)
        {
            var provider = await _mediator.Send(new GetProviderByUkprnQuery(request.Ukprn), cancellationToken);

            var tokens = new Dictionary<string, string>()
            {
                { "provider_organisation", provider.ProviderName },
                { "provider_name", request.ProviderFullName },
                { "employer_organisation", request.EmployerOrganisation },
                { "employer_name", request.EmployerFullName },
                { "invitation_link", $"{_configuration.EmployerAccountsBaseUrl}/service/register/{request.CorrelationId}" },
                { "unsubscribe_training_provider", $"{_configuration.EmployerAccountsBaseUrl}/service/unsubscribe/{request.CorrelationId}" },
                { "report_training_provider", $"{_configuration.EmployerAccountsBaseUrl}/report/trainingprovider/{request.CorrelationId}" }

            };

            await _publisher.Send(new SendEmailCommand(NotificationTemplateId, request.EmployerEmail, tokens));
        }
    }
}