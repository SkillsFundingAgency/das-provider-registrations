using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class UrlHelperExtensions
    {
        private static readonly string ProviderIdKey = "ProviderId";

        public static string ProviderAction(this IUrlHelper url, string actionName)
        {
            if (url.ActionContext.RouteData.Values.ContainsKey(ProviderIdKey) && url.ActionContext.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var controllerActionDescriptor = descriptor;
                var controllerName = controllerActionDescriptor.ControllerName;
                var providerId = url.ActionContext.RouteData.Values[ProviderIdKey].ToString();
                return url.Action(actionName, controllerName, new { ProviderId = providerId });
            }

            return url.Action(actionName);
        }
    }
}
