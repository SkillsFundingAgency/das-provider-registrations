using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProviderSharedUIConfiguration _providerSharedUiConfiguration;
        private readonly IConfiguration _configuration;

        public HomeController(ProviderSharedUIConfiguration providerSharedUiConfiguration, IConfiguration configuration)
        {
            _providerSharedUiConfiguration = providerSharedUiConfiguration;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("signout", Name = "provider-signout")]
        public IActionResult SignOutProvider()
        {
            var useDfESignIn = _configuration["UseDfESignIn"] != null && _configuration["UseDfESignIn"]
                .Equals("true", StringComparison.CurrentCultureIgnoreCase);

            var authScheme = useDfESignIn
                ? OpenIdConnectDefaults.AuthenticationScheme
                : WsFederationDefaults.AuthenticationScheme;

            return SignOut(
                new AuthenticationProperties
                {
                    AllowRefresh = true
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                authScheme);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/dashboard")]
        public IActionResult Dashboard()
        {
            return RedirectPermanent(_providerSharedUiConfiguration.DashboardUrl);
        }
    }
}
