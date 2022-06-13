using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationEventsViewModel
    {
        public List<InvitationEventViewModel> InvitationEvents { get; set; }

        public DateTime? InvitationSentDate { get; set; }

        public string EmployerOrganisation { get; set; }

        public bool InvitationResent => InvitationEvents.Any(x => x.EventType == EventTypeViewModel.InvitationResent);

        public DateTime? InvitationResentDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.InvitationResent)?.FirstOrDefault()?.Date;

        public DateTime? AgreementAcceptedDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.LegalAgreementSigned)?.FirstOrDefault()?.Date;

        public string AgreementAcceptedStatus => AgreementAcceptedDate != null ? AgreementAcceptedDate?.ToString("dd MMM yy") : "Legal agreement not accepted";

        public DateTime? AccountCreationStartedDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.AccountStarted)?.FirstOrDefault()?.Date;

        public string AccountCreationStartedStatus => AccountCreationStartedDate != null ? AccountCreationStartedDate?.ToString("dd MMM yy") : "Account creation not started";

        public DateTime? PayeSchemeAddedDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.PayeSchemeAdded)?.FirstOrDefault()?.Date;

        public string PayeSchemeAddedStatus => PayeSchemeAddedDate != null ? PayeSchemeAddedDate?.ToString("dd MMM yy") : "PAYE scheme not added";
    }
}
