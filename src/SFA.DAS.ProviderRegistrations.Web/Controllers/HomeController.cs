using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

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
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
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
