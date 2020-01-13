using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand
{
    public class SendInvitationEmailCommandHandler : AsyncRequestHandler<SendInvitationEmailCommand>
    {
        private readonly IMessageSession _publisher;
        private readonly IMediator _mediator;

        public SendInvitationEmailCommandHandler(IMediator mediator, IMessageSession publisher)
        {
            _publisher = publisher;
            _mediator = mediator;
        }

        protected override async Task Handle(SendInvitationEmailCommand request, CancellationToken cancellationToken)
        {
            var provider = await _mediator.Send(new GetProviderByUkprnQuery(request.Ukprn), cancellationToken);

            var tokens = new Dictionary<string, string>()
            {
                { "provider_organisation", provider.ProviderName },
                { "provider_name", request.ProviderFullName },
                { "employer_organisation", request.EmployerOrganisation },
                { "employer_name", request.EmployerFullName }
            };

            await _publisher.Send(new SendEmailCommand("InviteEmployerNotification_dev", request.EmployerEmail, string.Empty, tokens, string.Empty));
        }
    }
}