using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FanMatch.Models;
using FanMatch.Models.Data_Access;
using FanMatch.ViewModels;

namespace FanMatch.Controllers
{

    public enum MatchAction
    {
        Clear,
        Ban,
        Lock
    }

    [CustomBasicAuthorize]
    public class MatchController : Controller
    {
        private readonly Func<IMatchRepository> matchRepo;
        private readonly Func<IPersonRepository> personRepo;

        public MatchController(Func<IMatchRepository> matchRepo, Func<IPersonRepository> personRepo) {
            this.matchRepo = matchRepo;
            this.personRepo = personRepo;
        }

        public MatchController() : this(() => new MatchRepository(), () => new PersonRepository()) { }

        //
        // GET: /Match/

        public ActionResult Index()
        {
            using (var pRepo = this.personRepo())
            using (var mRepo = this.matchRepo())
            {
                var allPeople = pRepo.GetAll();
                var matcher = new Matcherizer(allPeople, mRepo.LoadAll());
                var matches = matcher.Matcherize();
                return View(new MatchResultViewModel(matches, allPeople));
            }
        }

        public ActionResult ChangeMatchStatus(int reader, int writer, MatchAction action)
        {
            using (var mRepo = this.matchRepo())
            {
                switch (action)
                {
                    case MatchAction.Ban:
                        mRepo.Ban(reader, writer);
                        break;
                    case MatchAction.Lock:
                        mRepo.Lock(reader, writer);
                        break;
                    case MatchAction.Clear:
                        mRepo.Clear(reader, writer);
                        break;
                }
            }
            return RedirectToAction("Index");
        }

    }
}
