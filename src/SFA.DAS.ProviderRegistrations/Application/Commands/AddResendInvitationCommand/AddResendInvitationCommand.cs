using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand
{
    public class AddResendInvitationCommand : IRequest
    {
        public long InvitationId { get; set; }
        public DateTime InvitationReSentDate { get; set; }
        

        public AddResendInvitationCommand(long invitationId, DateTime invitationReSentDate)
        {
            InvitationId = invitationId;
            InvitationReSentDate = invitationReSentDate;
        }
    }
}
