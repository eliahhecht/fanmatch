using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FanMatch.Models;

namespace FanMatch.Controllers
{

    [CustomBasicAuthorize]
    public class PersonController : Controller
    {
        public class PersonEditorViewModel
        {
            public Person Person;
            public IEnumerable<Fandom> AllFandoms;
        }

        private FanMatchDb db = new FanMatchDb();

        //
        // GET: /Person/

        public ViewResult Index()
        {
            return View(db.People.ToList());
        }

        //
        // GET: /Person/Details/5

        public ViewResult Details(int id)
        {
            Person person = db.People.Find(id);
            return View(person);
        }

        //
        // GET: /Person/Create

        public ActionResult Create()
        {
            var model = new PersonEditorViewModel
            {
                AllFandoms = db.Fandoms.ToList(),
                Person = new Person { Fandoms = new List<Fandom>() }
            };
            return View(model);
        } 

        //
        // POST: /Person/Create

        [HttpPost]
        public ActionResult Create(Person person, string fandomIds)
        {
            if (ModelState.IsValid)
            {
                var fandoms = ParseFandoms(fandomIds);
                person.Fandoms = fandoms;
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(person);
        }
        
        //
        // GET: /Person/Edit/5
 
        public ActionResult Edit(int id)
        {
            Person person = db.People.Find(id);
            
            var allFandoms = db.Fandoms.ToList();
            var model = new PersonEditorViewModel
            {
                Person = person,
                AllFandoms = allFandoms
            };
            return View(model);
        }

        public PartialViewResult _PersonTag(int id)
        {
            var person = db.People.Find(id);
            return PartialView(person);
        }

        //
        // POST: /Person/Edit/5

        [HttpPost]
        public ActionResult Edit(Person person, string fandomIds)
        {
            var fandoms = ParseFandoms(fandomIds);

            var entry = db.Entry(person);
            entry.State = EntityState.Modified;
            entry.Collection(e => e.Fandoms).Load(); // have to load the collection, or else setting it doesn't work (?!)

            person.Fandoms = fandoms;

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        private List<Fandom> ParseFandoms(string fandomIds)
        {
            var fandoms = fandomIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Distinct()
                .Select(i => db.Fandoms.Find(i))
                .ToList();
            return fandoms;
        }

        //
        // GET: /Person/Delete/5
 
        public ActionResult Delete(int id)
        {
            Person person = db.People.Find(id);
            return View(person);
        }

        //
        // POST: /Person/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}