using System;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public class InvitationEvent
    {
        public long Id { get; set; }

        public virtual Invitation Invitation { get; set; }

        public long? InvitationId { get; set; }

        public int EventType { get; set; }

        public DateTime? Date { get; set; }
        
        public InvitationEvent(long? invitationId, int eventType, DateTime? date)
        {
            InvitationId = invitationId;
            EventType = eventType;
            Date = date;
        }
    }
}
