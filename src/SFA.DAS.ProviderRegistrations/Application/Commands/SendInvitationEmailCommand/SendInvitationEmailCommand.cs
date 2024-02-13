namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand;

public class SendInvitationEmailCommand : IRequest
{
    public SendInvitationEmailCommand(long ukprn, string providerOrgName, string providerUserFullName, string organisation, string employerFullName, string email, string correlationId)
    {
        Ukprn = ukprn;
        ProviderUserFullName = providerUserFullName;
        ProviderOrgName = providerOrgName;
        EmployerOrganisation = organisation;
        EmployerFullName = employerFullName;
        EmployerEmail = email;
        CorrelationId = correlationId;
    }

    public string CorrelationId { get; }

    public long Ukprn { get; }

    public string ProviderOrgName { get; }

    public string ProviderUserFullName { get; }

    public string EmployerOrganisation { get; }

    public string EmployerFullName { get; }

    public string EmployerEmail { get; }
}