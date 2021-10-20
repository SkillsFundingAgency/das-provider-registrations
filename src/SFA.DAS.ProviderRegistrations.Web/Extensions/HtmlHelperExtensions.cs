using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString AddClassIfPropertyInError<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string errorClass)
        {
            var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;
            var expressionText = expressionProvider.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];

            if (state?.Errors == null || state.Errors.Count == 0)
            {
                return HtmlString.Empty;
            }

            return new HtmlString(errorClass);
        }

        public static IEnumerable<KeyValuePair<string, string>> GetModelErrorsInOrder<TModel>(
            this IHtmlHelper<TModel> htmlHelper)
        {
            return htmlHelper.ViewData.ModelState.Keys.SelectMany(
                key => htmlHelper.ViewData.ModelState[key].Errors.Select(
                    x => new KeyValuePair<string, string>(key, x.ErrorMessage))).OrderBy(
                o => htmlHelper.ViewData.ModelMetadata.Properties.Single(p => p.Name == o.Key).Order);
        }
    }
}
