using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
    public class TrainingProviderAllRolesAuthorizationHandler : AuthorizationHandler<TrainingProviderAllRolesRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITrainingProviderAuthorizationHandler _handler;
        private readonly IConfiguration _configuration;
        private readonly ProviderSharedUIConfiguration _providerSharedUiConfiguration;

        public TrainingProviderAllRolesAuthorizationHandler(
            ITrainingProviderAuthorizationHandler handler,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ProviderSharedUIConfiguration providerSharedUiConfiguration)
        {
            _handler = handler;
            _configuration = configuration;
            _providerSharedUiConfiguration = providerSharedUiConfiguration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingProviderAllRolesRequirement requirement)
        {
            var isStubProviderValidationEnabled = GetUseStubProviderValidationSetting();

            // logic to check if the provider is authorized if not redirect the user to 401 un-authorized page.
            if (!isStubProviderValidationEnabled && !(await _handler.IsProviderAuthorized(context, true)))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                httpContext?.Response.Redirect($"{_providerSharedUiConfiguration.DashboardUrl}/error/401");
            }

            context.Succeed(requirement);
        }

        private bool GetUseStubProviderValidationSetting()
        {
            var value = _configuration.GetSection("UseStubProviderValidation").Value;

            return value != null && bool.TryParse(value, out var result) && result;
        }
    }
}
