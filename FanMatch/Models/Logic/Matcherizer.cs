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
        private HashSet<Tuple<int, int>> banned;
        private HashSet<Tuple<int, int>> locked;
        private MatchResult res;

        public const int MAX_MATCHES_PER_PERSON = 2;

        public Matcherizer(IEnumerable<Person> people, IEnumerable<Match> existingMatches)
        {
            this.people = people;
            this.matchCountByPersonId = people.ToDictionary(p => p.Id, p => 0);
            this.matchablePeopleByFandomId = new Dictionary<int, List<Person>>();
            this.banned = new HashSet<Tuple<int, int>>();
            this.locked = new HashSet<Tuple<int, int>>();
            this.res = new MatchResult();

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

            foreach (var match in existingMatches.ToList())
            {
                if (match.IsLocked)
                {
                    matchCountByPersonId[match.Reader.Id]++;
                    matchCountByPersonId[match.Writer.Id]++;
                    match.Fandom = match.Reader.Fandoms.Intersect(match.Writer.Fandoms).FirstOrDefault();
                    this.res.LockedMatches.Add(match);
                }
                else if (match.IsBanned)
                {
                    this.banned.Add(Tuple.Create(match.Reader.Id, match.Writer.Id));
                    this.res.BannedMatches.Add(match);
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
                .OrderBy(p => p.Fandoms.Count());
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

        private bool BannedPair(Person a, Person b)
        {
            return this.banned.Contains(Tuple.Create(a.Id, b.Id))
                || this.banned.Contains(Tuple.Create(b.Id, a.Id));
        }

        private Match FindMatch(Fandom fandom, Person person)
        {
            var listForFandom = this.matchablePeopleByFandomId[fandom.Id];

            var other = listForFandom
                .Where(p => !this.BannedPair(person, p))
                .Where(p => this.HasRoomForMoreMatches(p))
                .OrderBy(p => this.matchCountByPersonId[p.Id])
                .FirstOrDefault(p => p.Complements(person));
            if (other == null)
            {
                return null;
            }
            var matchees = new[] { person, other };
            foreach (var p in matchees)
            {
                matchCountByPersonId[p.Id]++;

                if (!HasRoomForMoreMatches(p))
                {
                    listForFandom.Remove(p);
                }
            }

            Person reader, writer;
            if (person.IsReader && other.IsWriter)
            {
                reader = person;
                writer = other;
            }
            else
            {
                writer = person;
                reader = other;
            }


            return new Match { Fandom = fandom, Writer = writer, Reader = reader };
        }

    }
}