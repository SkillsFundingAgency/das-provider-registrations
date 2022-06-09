using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationResentCommand
{
    public class UpdateInvitationResentCommand : IRequest
    {
        public long InvitationId { get; set; }
        public DateTime InvitationReSentDate { get; set; }
        

        public UpdateInvitationResentCommand(long invitationId, DateTime invitationReSentDate)
        {
            InvitationId = invitationId;
            InvitationReSentDate = invitationReSentDate;
        }
    }
}
