using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FanMatch
{
    public class HackAuthAttribute : ActionFilterAttribute
    {
        public const string CookieName = "061faedc-988d-4a96-85df-d3c37000b420";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies[CookieName] == null)
            {
                filterContext.HttpContext.Response.Redirect("/");
            }
        }
    }
}