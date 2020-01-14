using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand
{
    public class SendInvitationEmailCommand : IRequest
    {
        public SendInvitationEmailCommand(long ukprn, string providerFullName, string organisation, string employerFullName, string email, string correlationId)
        {
            Ukprn = ukprn;
            ProviderFullName = providerFullName;
            EmployerOrganisation = organisation;
            EmployerFullName = employerFullName;
            EmployerEmail = email;
            CorrelationId = correlationId;
        }

        public long Ukprn { get; }

        public string ProviderFullName { get; }

        public string EmployerOrganisation { get; }

        public string EmployerFullName { get; }

        public string EmployerEmail { get; }

        public string CorrelationId { get; }
    }
}