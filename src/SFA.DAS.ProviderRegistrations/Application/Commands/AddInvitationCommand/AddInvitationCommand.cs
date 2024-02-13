namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;

public class AddInvitationCommand : IRequest<string>
{
    public AddInvitationCommand(long ukprn, string userRef, string providerOrgName, string providerUserFullName, string organisation, string firstName, string lastName, string email)
    {
        Ukprn = ukprn;
        UserRef = userRef;
        EmployerOrganisation = organisation;
        EmployerFirstName = firstName;
        EmployerLastName = lastName;
        EmployerEmail = email;
        ProviderOrganisationName = providerOrgName;
        ProviderUserFullName = providerUserFullName;
    }

    public long Ukprn { get; }

    public string UserRef { get; }

    public string EmployerOrganisation { get; }

    public string EmployerFirstName { get; }

    public string EmployerLastName { get; }

    public string EmployerEmail { get; }

    public string ProviderOrganisationName { get; }

    public string ProviderUserFullName { get; }
}