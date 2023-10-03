using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using SFA.DAS.ProviderRegistrations.Services;
using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
    /// <summary>
    /// Interface to define contracts related to Training Provider Authorization Handlers.
    /// </summary>
    public interface ITrainingProviderAuthorizationHandler
    {
        /// <summary>
        /// Contract to check is the logged in Provider is a valid Training Provider. 
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext.</param>
        /// <param name="allowAllUserRoles">boolean.</param>
        /// <returns>boolean.</returns>
        Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context, bool allowAllUserRoles);
    }

    ///<inheritdoc cref="ITrainingProviderAuthorizationHandler"/>
    public class TrainingProviderAuthorizationHandler : ITrainingProviderAuthorizationHandler
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITrainingProviderApiClient _trainingProviderApiClient;

        public TrainingProviderAuthorizationHandler(
            ITrainingProviderApiClient trainingProviderApiClient,
            IAuthenticationService authenticationService)
        {
            _trainingProviderApiClient = trainingProviderApiClient;
            _authenticationService = authenticationService;
        }

        // <inherit-doc />
        public async Task<bool> IsProviderAuthorized(AuthorizationHandlerContext context, bool allowAllUserRoles)
        {
            if (!_authenticationService.IsUserAuthenticated())
                return false;

            var ukprn = GetProviderId();
            var providerDetails = await _trainingProviderApiClient.GetProviderDetails(ukprn);

            // Condition to check if the Provider Details has permission to access Apprenticeship Services based on the property value "CanAccessApprenticeshipService" set to True.
            return providerDetails is { CanAccessApprenticeshipService: true };
        }

        private long GetProviderId()
        {
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
    }
}
