using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand
{
    public class UpdateInvitationCommand :  IRequest
    {
        public string CorrelationId { get; }

        public UpdateInvitationCommand(string correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
