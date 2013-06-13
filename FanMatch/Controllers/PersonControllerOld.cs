using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FanMatch.Models;

namespace FanMatch.Controllers
{
    public class PersonControllerOld : Controller
    {
        //
        // GET: /Person/

        private IPersonRepository repo;

        public PersonControllerOld(IPersonRepository repo)
        {
            this.repo = repo;
        }

        public PersonControllerOld()
        {
            this.repo = new PersonRepository();
        }

        public ActionResult Index()
        {
            var allThePeople = repo.GetAll();
            return View(allThePeople);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            var newPerson = new Person { Name = name };
            repo.Create(newPerson);
            return RedirectToAction("Index");
        }
    }
}
