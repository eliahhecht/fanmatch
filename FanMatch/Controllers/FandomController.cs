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
    public class FandomController : Controller
    {
        private readonly FanMatchDb db = new FanMatchDb();


        //
        // GET: /Fandom/

        public ViewResult Index()
        {
            return View();
        }

        public PartialViewResult _FandomTable()
        {
            return PartialView(db.Fandoms.ToList());
        }

        //
        // GET: /Fandom/Details/5

        public ViewResult Details(int id)
        {
            Fandom fandom = db.Fandoms.Find(id);
            return View(fandom);
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
            if (ModelState.IsValid)
            {
                db.Fandoms.Add(fandom);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(fandom);
        }
        
        //
        // GET: /Fandom/Edit/5
 
        public ActionResult Edit(int id)
        {
            Fandom fandom = db.Fandoms.Find(id);
            return View(fandom);
        }

        //
        // POST: /Fandom/Edit/5

        [HttpPost]
        public ActionResult Edit(Fandom fandom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fandom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fandom);
        }

        //
        // GET: /Fandom/Delete/5
 
        public ActionResult Delete(int id)
        {
            Fandom fandom = db.Fandoms.Find(id);
            return View(fandom);
        }

        //
        // POST: /Fandom/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Fandom fandom = db.Fandoms.Find(id);
            db.Fandoms.Remove(fandom);
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