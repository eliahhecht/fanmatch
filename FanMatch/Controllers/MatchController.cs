using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FanMatch.Models;

namespace FanMatch.Controllers
{
    public class MatchController : Controller
    {
        private FanMatchDb db = new FanMatchDb();

        //
        // GET: /Match/

        public ActionResult Index()
        {
            var matcher = new Matcherizer(db.People.ToList());
            var matches = matcher.Matcherize();
            return View(matches);
        }

    }
}
