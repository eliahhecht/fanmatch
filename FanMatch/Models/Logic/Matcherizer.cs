using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Logic;

namespace FanMatch.Models
{
    public class Matcherizer
    {
        private IEnumerable<Person> people;

        public Matcherizer(IEnumerable<Person> people)
        {
            this.people = people;
        }

        public MatchResult Matcherize()
        {
            var allThePeople = people
                .OrderBy(p => p.Fandoms.Count())
                .ToList();

            var alreadyMatched = new HashSet<int>();

            var res = new MatchResult();

            foreach (var person in allThePeople)
            {
                if (alreadyMatched.Contains(person.Id))
                {
                    continue;
                }

                var match = allThePeople
                    .Where(p => !alreadyMatched.Contains(p.Id))
                    .Where(p => p.Id != person.Id)
                    .Where(p => p.Complements(person))
                    .Where(p => p.Fandoms.Intersect(person.Fandoms).Any())
                    .FirstOrDefault();

                if (match != null)
                {
                    alreadyMatched.Add(person.Id);
                    alreadyMatched.Add(match.Id);

                    var reader = person.IsReader
                        ? person
                        : match;

                    var writer = reader == person
                        ? match
                        : person;

                    res.Matches.Add(new Match
                    {
                        Fandom = person.Fandoms.Intersect(match.Fandoms).First(),
                        Reader = reader,
                        Writer = writer
                    });
                }
                else
                {
                    res.UnmatchedPeople.Add(person);
                }

            }

            return res;
        }

    }
}