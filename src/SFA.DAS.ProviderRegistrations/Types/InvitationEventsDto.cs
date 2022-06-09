using System;

namespace SFA.DAS.ProviderRegistrations.Types
{
    public class InvitationEventsDto
    {
        public long Id { get; set; }

        public long InvitationId { get; set; }

        public DateTime? InvitationReSentDate { get; set; }

        public DateTime? AccountCreationStartedDate { get; set; }

        public DateTime? PayeSchemeAddedDate { get; set; }

        public DateTime? AgreementAcceptedDate { get; set; }

        public DateTime? InvitationSentDate { get; set; }

        public string EmployerOrganisation { get; set;}
    }
}
