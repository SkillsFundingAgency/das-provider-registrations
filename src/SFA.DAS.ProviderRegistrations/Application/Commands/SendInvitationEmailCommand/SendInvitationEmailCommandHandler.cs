using NServiceBus;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.ProviderRegistrations.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand;

public class SendInvitationEmailCommandHandler : IRequestHandler<SendInvitationEmailCommand>
{
    private readonly string _notificationTemplateId = "ProviderInviteEmployerNotification";
    private readonly IMessageSession _publisher;
    private readonly ProviderRegistrationsSettings _configuration;
    private readonly ILogger<SendInvitationEmailCommandHandler> _logger;

    public SendInvitationEmailCommandHandler(IMessageSession publisher, ProviderRegistrationsSettings configuration, ILogger<SendInvitationEmailCommandHandler> logger)
    {
        _publisher = publisher;
        _configuration = configuration;
        _logger = logger;
        
        if (_configuration.UseGovLogin)
        {
            _notificationTemplateId = _configuration.ResourceEnvironmentName.ToLower() == "prd" 
                ? "9dc52d84-0ee5-4755-b836-f4e71ae2a326" : "02818d7b-cea1-4445-8b16-5a27f40ddaf6";
        }
    }

    public Task Handle(SendInvitationEmailCommand request, CancellationToken cancellationToken)
    {
        var tokens = new Dictionary<string, string>
        {
            { "provider_organisation", request.ProviderOrgName },
            { "provider_name", request.ProviderUserFullName },
            { "employer_organisation", request.EmployerOrganisation },
            { "employer_name", request.EmployerFullName },
            { "invitation_link", $"{_configuration.EmployerAccountsBaseUrl}/service/register/{request.CorrelationId}" },
            { "unsubscribe_training_provider", $"{_configuration.EmployerAccountsBaseUrl}/service/unsubscribe/{request.CorrelationId}" },
            { "report_training_provider", $"{_configuration.EmployerAccountsBaseUrl}/report/trainingprovider/{request.CorrelationId}" }
        };
        
        //_logger.LogInformation("Sending invitation email to employer via SendInvitationEmailCommand. TemplateId: {TemplateId}, EmployerEmail: {Email}, tokens: {Tokens}", _notificationTemplateId, request.EmployerEmail, JsonSerializer.Serialize(tokens));
        
        _publisher.Send(new SendEmailCommand(_notificationTemplateId, request.EmployerEmail, tokens));

        return Task.CompletedTask;
    }
}