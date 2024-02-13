using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
    public class MinimumServiceClaimRequirement : IAuthorizationRequirement
    {
        public ServiceClaim MinimumServiceClaim { get; set; }

        public MinimumServiceClaimRequirement(ServiceClaim minimumServiceClaim)
        {
            MinimumServiceClaim = minimumServiceClaim;
        }
    }
}
