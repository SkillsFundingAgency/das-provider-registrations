using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRegistrations.Types
{
    public class InvitationEvents
    {
        public List<InvitationEventDto> invitationEventDtos;
    }

    public class InvitationEventDto
    {
        public long Id { get; set; }

        public int EventType { get; set; }

        public DateTime? Date { get; set; }
    }
}
