using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FanMatch.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult SetCookie(string password)
        {
            if (password == "podficplease")
            {
                Response.Cookies.Add(new HttpCookie("auth", "true"));
            }

            return RedirectToAction("Index", "Match");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
