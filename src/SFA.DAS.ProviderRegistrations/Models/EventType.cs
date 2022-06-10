using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public enum EventType
    {
        [Display(Name = "Invitation Resent")] InvitationResent = 0,

        [Display(Name = "Account creation started")] AccountStarted = 1,

        [Display(Name = "PAYE scheme added")] PayeSchemeAdded = 2,

        [Display(Name = "Legal agreement accepted")] LegalAgreementSigned = 3,
    }
}
