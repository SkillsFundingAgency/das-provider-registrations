using System;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.Web.Extensions;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationViewModel
    {
        public long Id { get; set; }

        public Guid Reference { get; set; }

        public string EmployerOrganisation { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public string Name => $"{EmployerFirstName} {EmployerLastName}";

        public string EmployerEmail { get; set; }

        public string State { get; set; }

        public DateTime SentDate { get; set; }

        public bool ShowResendInvitationLink => State == InvitationStatus.InvitationSent.GetDisplayName();

        public bool ViewStatusLink => (State == InvitationStatus.AccountStarted.GetDisplayName() ||
            State == InvitationStatus.PayeSchemeAdded.GetDisplayName() ||
            State == InvitationStatus.LegalAgreementSigned.GetDisplayName());            
    }
}
