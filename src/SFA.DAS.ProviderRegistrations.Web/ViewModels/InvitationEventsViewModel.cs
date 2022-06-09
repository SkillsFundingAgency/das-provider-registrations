using System;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationEventsViewModel
    {
        public long InvitationId { get; set; }

        public DateTime? InvitationReSentDate { get; set; }

        public DateTime? AccountCreationStartedDate { get; set; }

        public DateTime? PayeSchemeAddedDate { get; set; }

        public DateTime? AgreementAcceptedDate { get; set; }

        public DateTime InvitationSentDate { get; set; }

        public string EmployerOrganisation { get; set; }

        public string AgreementAcceptedStatus => AgreementAcceptedDate != null ? AgreementAcceptedDate?.ToString("dd MMM yy") : "Legal agreement not accepted";

        public string AccountCreationStartedStatus => AccountCreationStartedDate != null ? AccountCreationStartedDate?.ToString("dd MMM yy") : "Account creation not started";

        public string PayeSchemeAddedStatus => PayeSchemeAddedDate != null ? PayeSchemeAddedDate?.ToString("dd MMM yy") : "PAYE scheme not added";

        public bool InvitationResent => InvitationReSentDate != null;
    }
}
