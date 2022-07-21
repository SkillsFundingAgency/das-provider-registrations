using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationEventsViewModel
    {
        public const string DateFormat = "dd MMMM yyyy";
        public List<InvitationEventViewModel> InvitationEvents { get; set; }

        public InvitationViewModel Invitation { get; set; }

        public DateTime? InvitationSentDate { get; set; }

        public string EmployerOrganisation { get; set; }

        public bool InvitationResent => InvitationEvents.Any(x => x.EventType == EventTypeViewModel.InvitationResent);

        public DateTime? InvitationResentDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.InvitationResent)?.FirstOrDefault()?.Date;

        public DateTime? AgreementAcceptedDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.LegalAgreementSigned)?.FirstOrDefault()?.Date;

        public string AgreementAcceptedStatus => AgreementAcceptedDate != null ? AgreementAcceptedDate?.ToString(DateFormat) : GetAgreementAcceptedText();

        public DateTime? AccountCreationStartedDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.AccountStarted)?.FirstOrDefault()?.Date;

        public string AccountCreationStartedStatus => AccountCreationStartedDate != null ? AccountCreationStartedDate?.ToString(DateFormat) : GetAccountCreationStartedText();

        public DateTime? PayeSchemeAddedDate => InvitationEvents?.Where(x => x.EventType == EventTypeViewModel.PayeSchemeAdded)?.FirstOrDefault()?.Date;

        public string PayeSchemeAddedStatus => PayeSchemeAddedDate != null ? PayeSchemeAddedDate?.ToString(DateFormat) : GetPaymentSchemeText() ;

        private string GetAccountCreationStartedText()
        {
            if (Status.HasValue)
            {
                switch (Status)
                {
                    case InvitationStatusViewModel.PayeSchemeAdded:
                    case InvitationStatusViewModel.LegalAgreementSigned:
                    case InvitationStatusViewModel.InvitationComplete:
                    case InvitationStatusViewModel.AccountStarted:
                        return "Started";
                }
            }

            return "Account creation not started";
        }

        private string GetAgreementAcceptedText()
        {
            if (Status.HasValue)
            {
                switch (Status)
                {
                    case InvitationStatusViewModel.LegalAgreementSigned:
                    case InvitationStatusViewModel.InvitationComplete:
                        return "Accepted";
                }
            }

            return "Legal agreement not accepted";
        }

        private string GetPaymentSchemeText()
        {
            if (Status.HasValue)
            {
                switch (Status)
                {
                    case InvitationStatusViewModel.PayeSchemeAdded:
                    case InvitationStatusViewModel.LegalAgreementSigned:
                    case InvitationStatusViewModel.InvitationComplete:
                        return "Added";
                }
            }

            return "PAYE scheme not added";
        }

        public InvitationStatusViewModel? Status => Invitation?.Status; 
    }
}
