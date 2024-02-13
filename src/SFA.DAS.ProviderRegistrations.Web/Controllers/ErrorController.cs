using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;
using System;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    [AllowAnonymous]
    [HideNavigationBar(hideAccountHeader: false, hideNavigationLinks: true)]
    public class ErrorController : Controller
    {
        private readonly IConfiguration _configuration;

        public ErrorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("error/{statuscode?}")]
        public IActionResult Error(int? statusCode)
        {
            var useDfESignIn = _configuration["UseDfESignIn"] != null && _configuration["UseDfESignIn"]
                .Equals("true", StringComparison.CurrentCultureIgnoreCase);

            switch (statusCode)
            {
                case 403:
                    return View("403", new Error403ViewModel(_configuration["ResourceEnvironmentName"])
                    {
                        UseDfESignIn = useDfESignIn,
                    });
                case 404:
                    return View(statusCode.ToString());
                default:
                    return View();
            }
        }
    }
}