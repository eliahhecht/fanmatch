using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FanMatch
{
    public class HackAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["auth"] == null)
            {
                filterContext.HttpContext.Response.Redirect("/");
            }
        }
    }
}