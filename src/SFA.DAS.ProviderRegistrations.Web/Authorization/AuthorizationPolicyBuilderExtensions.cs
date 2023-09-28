using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
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
                    policy.Requirements.Add(new TrainingProviderAllRolesRequirement()); //Policy requirement to check if the signed provider is a Main or Employer Profile.
                });

                options.AddPolicy(PolicyNames.HasContributorOrAbovePermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderClaims.Service);
                    policy.RequireClaim(ProviderClaims.Ukprn);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAC));
                    policy.Requirements.Add(new TrainingProviderAllRolesRequirement()); //Policy requirement to check if the signed provider is a Main or Employer Profile.
                });

                options.AddPolicy(PolicyNames.HasContributorWithApprovalOrAbovePermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderClaims.Service);
                    policy.RequireClaim(ProviderClaims.Ukprn);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAB));
                    policy.Requirements.Add(new TrainingProviderAllRolesRequirement()); //Policy requirement to check if the signed provider is a Main or Employer Profile.
                });

                options.AddPolicy(PolicyNames.HasAccountOwnerPermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderClaims.Service);
                    policy.RequireClaim(ProviderClaims.Ukprn);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAA));
                    policy.Requirements.Add(new TrainingProviderAllRolesRequirement()); //Policy requirement to check if the signed provider is a Main or Employer Profile.
                });
            });

            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IAuthorizationHandler, ProviderHandler>();
            services.AddTransient<IAuthorizationHandler, MinimumServiceClaimRequirementHandler>();
            services.AddSingleton<ITrainingProviderAuthorizationHandler, TrainingProviderAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, TrainingProviderAllRolesAuthorizationHandler>();
        }
    }
}