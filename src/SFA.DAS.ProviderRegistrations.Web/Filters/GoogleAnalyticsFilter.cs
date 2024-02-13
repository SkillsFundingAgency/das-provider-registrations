using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }

            controller.ViewBag.GaData = PopulateGaData(context);

            base.OnActionExecuting(context);
        }

        private static GaData PopulateGaData(ActionExecutingContext context)
        {
            var ukPrn = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ProviderClaims.Ukprn))?.Value;
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ProviderClaims.Upn))?.Value;

            return new GaData
            {
                UkPrn = ukPrn,
                UserId = userId
            };
        }
    }
}
