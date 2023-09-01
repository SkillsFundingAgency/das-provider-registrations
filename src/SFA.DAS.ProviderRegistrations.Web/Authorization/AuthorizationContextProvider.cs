using SFA.DAS.Authorization.Context;
using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
    public class AuthorizationContextProvider : IAuthorizationContextProvider
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationContextProvider(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            return authorizationContext;
        }

        private long? GetUkrpn()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                return null;
            }

            if (!_authenticationService.TryGetUserClaimValue(ProviderClaims.Ukprn, out var ukprnClaimValue))
            {
                throw new UnauthorizedAccessException($"Failed to get value for claim '{ProviderClaims.Ukprn}'");
            }

            if (!long.TryParse(ukprnClaimValue, out var ukprn))
            {
                throw new UnauthorizedAccessException($"Failed to parse value '{ukprnClaimValue}' for claim '{ProviderClaims.Ukprn}'");
            }

            return ukprn;
        }

        private string GetUserEmail()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                return null;
            }

            if (!_authenticationService.TryGetUserClaimValue(ProviderClaims.Email, out var userEmail))
            {
                throw new UnauthorizedAccessException($"Failed to get value for claim '{ProviderClaims.Email}'");
            }

            return userEmail;
        }
    }
}