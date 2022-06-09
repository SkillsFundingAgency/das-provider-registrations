using System;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public class InvitationEvents
    {
        public long Id { get; set; }

        public long? InvitationId { get; set; }        

        public DateTime? InvitationReSentDate { get; set; }

        public DateTime? AccountCreationStartedDate { get; set; }

        public DateTime? PayeSchemeAddedDate { get; set; }

        public DateTime? AgreementAcceptedDate { get; set; }

        public InvitationEvents(long? invitationId, DateTime? invitationReSentDate, DateTime? accountCreationStartedDate, DateTime? payeSchemeAddedDate, DateTime? agreementAcceptedDate)
        {
            InvitationId = invitationId;
            InvitationReSentDate = invitationReSentDate;
            AccountCreationStartedDate = accountCreationStartedDate;
            PayeSchemeAddedDate = payeSchemeAddedDate;
            AgreementAcceptedDate = agreementAcceptedDate;
        }
    }
}
