using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand
{
    public class UpdateInvitationCommand :  IRequest
    {
        public string CorrelationId { get; set; }

        public string EmployerOrganisation { get; private set; }

        public string EmployerFirstName { get; private set; }

        public string EmployerLastName { get; private set; }

        public UpdateInvitationCommand(string correlationId, string employerOrganisation, string employerFirstName, string employerLastName)
        {
            CorrelationId = correlationId;
            EmployerOrganisation = employerOrganisation;
            EmployerFirstName = employerFirstName;
            EmployerLastName = employerLastName;
        }
    }
}
