using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FanMatch.Models;
using FanMatch.ViewModels;

namespace FanMatch.Controllers
{ 
    public class FandomController : Controller
    {
        private readonly Func<IFandomRepository> getDb;

        public FandomController()
            : this(() => new FandomRepository()) { }

        public FandomController(Func<IFandomRepository> repo)
        {
            this.getDb = repo;
        }

        //
        // GET: /Fandom/

        public ViewResult Index()
        {
            return View();
        }

        public PartialViewResult _FandomTable()
        {
            using (var db = getDb())
            {
                var models = db.GetAll().Select(m => new FandomViewModel(m)).ToList();
                return PartialView(models);
            }
        }

        //
        // GET: /Fandom/Details/5

        public ViewResult Details(int id)
        {
            using (var db = getDb())
            {
                var fandom = db.GetById(id);
                return View(new FandomViewModel(fandom));
            }
        }

        //
        // GET: /Fandom/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Fandom/Create

        [HttpPost]
        public ActionResult Create(Fandom fandom)
        {
            using (var db = getDb())
            {
                if (db.GetByName(fandom.Name) != null)
                {
                    ModelState.AddModelError("Name", "There is already a fandom by this name");
                }
                
                if (ModelState.IsValid)
                {
                    db.Create(fandom);
                    return RedirectToAction("Create");
                }

                return View(fandom);
            }
        }
        
        //
        // GET: /Fandom/Edit/5
 
        public ActionResult Edit(int id)
        {
            using (var db = getDb())
            {
                Fandom fandom = db.GetById(id);
                return View(fandom);
            }
        }

        //
        // POST: /Fandom/Edit/5

        [HttpPost]
        public ActionResult Edit(Fandom fandom)
        {
            using (var db = getDb())
            {
                if (ModelState.IsValid)
                {
                    db.Update(fandom);
                    return RedirectToAction("Index");
                }
                return View(fandom);
            }
        }

        //
        // GET: /Fandom/Delete/5
 
        public ActionResult Delete(int id)
        {
            using (var db = getDb())
            {
                Fandom fandom = db.GetById(id);
                return View(fandom);
            }
        }

        //
        // POST: /Fandom/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = getDb())
            {
                db.Delete(id);
                return RedirectToAction("Index");
            }
        }
    }
}