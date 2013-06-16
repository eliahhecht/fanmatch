using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace FanMatch
{
    public static class Helpers
    {
        public static MvcHtmlString MenuLink(
    this HtmlHelper htmlHelper,
    string linkText,
    string actionName,
    string controllerName
)
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            if (controllerName == currentController)
            {
                return htmlHelper.ActionLink(
                    linkText,
                    actionName,
                    controllerName,
                    null,
                    new
                    {
                        @class = "active"
                    });
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }

        public static string ActiveIfCurrentController(this HtmlHelper helper, string controller)
        {
            string currentController = helper.ViewContext.RouteData.GetRequiredString("controller");
            return controller == currentController ? "active" : string.Empty;
        }

        public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable.ToArray());
        }

    }
}