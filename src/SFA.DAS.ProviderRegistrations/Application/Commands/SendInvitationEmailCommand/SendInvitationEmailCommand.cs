using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand
{
    public class SendInvitationEmailCommand : IRequest
    {
        public SendInvitationEmailCommand(long ukprn, string providerFullName, string organisation, string employerFullName, string email)
        {
            Ukprn = ukprn;
            ProviderFullName = providerFullName;
            EmployerOrganisation = organisation;
            EmployerFullName = employerFullName;
            EmployerEmail = email;
        }

        public long Ukprn { get; }

        public string ProviderFullName { get; }

        public string EmployerOrganisation { get; }

        public string EmployerFullName { get; }
                
        public string EmployerEmail { get; }
    }
}