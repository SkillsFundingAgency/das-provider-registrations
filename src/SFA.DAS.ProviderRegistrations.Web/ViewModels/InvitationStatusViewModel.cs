using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public enum InvitationStatusViewModel
    {
        [Display(Name = "Account creation not started")] InvitationSent = 0,

        [Display(Name = "Account creation started")] AccountStarted = 1,

        [Display(Name = "PAYE scheme added")] PayeSchemeAdded = 2,

        [Display(Name = "Legal agreement accepted")] LegalAgreementSigned = 3,

        [Display(Name = "Legal agreement accepted")] InvitationComplete = 4
    }     
}
