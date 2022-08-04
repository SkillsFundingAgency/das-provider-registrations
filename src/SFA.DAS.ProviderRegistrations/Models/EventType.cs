using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public enum EventType
    {
        InvitationResent = 0,

        AccountStarted = 1,

        PayeSchemeAdded = 2,

        LegalAgreementSigned = 3,
        AccountProviderAdded = 4,
    }
}
