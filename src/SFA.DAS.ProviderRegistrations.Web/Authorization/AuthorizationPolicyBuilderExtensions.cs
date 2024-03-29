﻿using Microsoft.AspNetCore.Mvc.Infrastructure;
using SFA.DAS.Authorization.Context;
using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization;

public static class AuthorizationPolicy
{
    public static void AddAuthorizationService(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.ProviderPolicyName, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ProviderClaims.Service);
                policy.RequireClaim(ProviderClaims.Ukprn);
                policy.Requirements.Add(new ProviderRequirement());
            });

            options.AddPolicy(PolicyNames.HasViewerOrAbovePermission, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ProviderClaims.Service);
                policy.RequireClaim(ProviderClaims.Ukprn);
                policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAV));
                policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
            });

            options.AddPolicy(PolicyNames.HasContributorOrAbovePermission, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ProviderClaims.Service);
                policy.RequireClaim(ProviderClaims.Ukprn);
                policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAC));
                policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
            });

            options.AddPolicy(PolicyNames.HasContributorWithApprovalOrAbovePermission, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ProviderClaims.Service);
                policy.RequireClaim(ProviderClaims.Ukprn);
                policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAB));
                policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
            });

            options.AddPolicy(PolicyNames.HasAccountOwnerPermission, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ProviderClaims.Service);
                policy.RequireClaim(ProviderClaims.Ukprn);
                policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAA));
                policy.Requirements.Add(new TrainingProviderAllRolesRequirement());
            });
        });

        services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
        services.AddTransient<IAuthorizationHandler, ProviderHandler>();
        services.AddTransient<IAuthorizationHandler, MinimumServiceClaimRequirementHandler>();
        services.AddTransient<IAuthorizationContextProvider, AuthorizationContextProvider>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<ITrainingProviderAuthorizationHandler, TrainingProviderAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, TrainingProviderAllRolesAuthorizationHandler>();
    }
}