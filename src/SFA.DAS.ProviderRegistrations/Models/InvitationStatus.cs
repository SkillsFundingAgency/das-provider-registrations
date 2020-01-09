using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public enum InvitationStatus
    {
        [Display(Name = "Invitation sent")] InvitationSent = 0,

        [Display(Name = "Account started")] AccountStarted = 1,

        [Display(Name = "PAYE scheme added")] PayeSchemeAdded = 2,

        [Display(Name = "Legal agreement accepted")] LegalAgreementSigned = 3,

        [Display(Name = "Legal agreement accepted")] InvitationComplete = 4
    }
}
