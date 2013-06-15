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
        private Dictionary<int, int> matchCountByPersonId;
        private Dictionary<int, List<Person>> matchablePeopleByFandomId;

        public const int MAX_MATCHES_PER_PERSON = 2;

        public Matcherizer(IEnumerable<Person> people)
        {
            this.people = people;
            this.matchCountByPersonId = people.ToDictionary(p => p.Id, p => 0);
            this.matchablePeopleByFandomId = new Dictionary<int, List<Person>>();

            foreach (var person in people)
            {
                foreach (var fandom in person.Fandoms)
                {
                    if (!matchablePeopleByFandomId.ContainsKey(fandom.Id))
                    {
                        matchablePeopleByFandomId[fandom.Id] = new List<Person>();
                    }
                    matchablePeopleByFandomId[fandom.Id].Add(person);
                }
            }
        }

        private bool HasRoomForMoreMatches(Person p)
        {
            var count = this.matchCountByPersonId[p.Id];
            return count == 0 || (count < MAX_MATCHES_PER_PERSON && p.CanMatchMultiple);
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
                if (!HasRoomForMoreMatches(person))
                {
                    continue;
                }


                foreach (var fandom in person.Fandoms)
                {
                    var match = FindMatch(fandom, person);
                    if (match != null)
                    {
                        Console.WriteLine("Matched {0} and {1} on fandom {2}", match.Reader.Id, match.Writer.Id, fandom.Id);
                        res.Matches.Add(match);
                        break;
                    }
                }

                if (this.matchCountByPersonId[person.Id] == 0)
                {
                    Console.WriteLine("Couldn't match " + person.Id);
                    res.UnmatchedPeople.Add(person);
                }

            }

            return res;
        }

        private Match FindMatch(Fandom fandom, Person person)
        {
            var listForFandom = this.matchablePeopleByFandomId[fandom.Id];

            var other = listForFandom
                .OrderBy(p => this.matchCountByPersonId[p.Id])
                .FirstOrDefault(p => p.Complements(person));
            if (other == null)
            {
                return null;
            }
            var matchees = new[] { person, other };
            Person reader = null;
            Person writer = null;
            foreach (var p in matchees)
            {
                matchCountByPersonId[p.Id]++;

                if (!HasRoomForMoreMatches(p))
                {
                    listForFandom.Remove(p);
                }
                if (reader == null && p.IsReader)
                {
                    reader = p;
                }
                else if (writer == null && p.IsWriter)
                {
                    writer = p;
                }
            }

            if (reader == null || writer == null)
            {
                throw new Exception("This shouldn't have happened: complementary match without both a reader and writer");
            }

            return new Match { Fandom = fandom, Writer = writer, Reader = reader };
        }

    }
}