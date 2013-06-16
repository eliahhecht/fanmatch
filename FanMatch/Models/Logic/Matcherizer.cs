﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Logic;

namespace FanMatch.Models
{
    public class MatchDict
    {
        private HashSet<Tuple<int, int>> matches = new HashSet<Tuple<int, int>>();

        public void Add(int a, int b)
        {
            this.matches.Add(Tuple.Create(a, b));
        }

        public bool Contains(int a, int b)
        {
            return matches.Contains(Tuple.Create(a, b))
                || matches.Contains(Tuple.Create(b, a));
        }
    }

    public class Matcherizer
    {
        private IEnumerable<Person> people;
        private Dictionary<int, int> matchCountByPersonId;
        private Dictionary<int, List<Person>> matchablePeopleByFandomId;
        private MatchDict banned;
        private MatchDict locked;
        private MatchDict alreadyMatched;
        private MatchResult res;

        public const int MAX_MATCHES_PER_PERSON = 2;

        public Matcherizer(IEnumerable<Person> people, IEnumerable<Match> existingMatches)
        {
            this.people = people;
            this.matchCountByPersonId = people.ToDictionary(p => p.Id, p => 0);
            this.matchablePeopleByFandomId = new Dictionary<int, List<Person>>();
            this.banned = new MatchDict();
            this.locked = new MatchDict();
            this.alreadyMatched = new MatchDict();
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
                    this.alreadyMatched.Add(match.Reader.Id, match.Writer.Id);
                }
                else if (match.IsBanned)
                {
                    this.banned.Add(match.Reader.Id, match.Writer.Id);
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


                foreach (var other in allThePeople.OrderBy(p => p.Fandoms.Count()))
                {
                    var match = FindMatch(other, person);
                    if (match != null)
                    {
                        Console.WriteLine("Matched {0} and {1}", match.Reader.Id, match.Writer.Id);
                        res.Matches.Add(match);
                        alreadyMatched.Add(match.Reader.Id, match.Writer.Id);
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
            return this.banned.Contains(a.Id, b.Id);
        }

        private Match FindMatch(Person other, Person person)
        {

            if (this.BannedPair(other, person)
                ||!HasRoomForMoreMatches(other)
                ||this.alreadyMatched.Contains(person.Id, other.Id)
                ||!person.Complements(other)
                ||!other.Fandoms.Intersect(person.Fandoms).Any())
            {
                return null;
            }

           
            var matchees = new[] { person, other };
            foreach (var p in matchees)
            {
                matchCountByPersonId[p.Id]++;
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


            return new Match { Writer = writer, Reader = reader };
        }

    }
}