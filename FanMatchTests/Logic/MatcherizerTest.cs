using FanMatch.Models;
using FanMatch.Models.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FanMatchTests.Logic
{
    public class MatcherizerTest
    {
        private IdGenerator idGen = new IdGenerator();
        private List<Person> people;
        private List<Match> existingMatches;

        [SetUp]
        public void Setup()
        {
            this.people = new List<Person>();
            this.universalFandom = MakeFandom();
            this.existingMatches = new List<Match>();
        }

        private Person MakePerson(ICollection<Fandom> fandoms = null, bool reader = false, bool writer = false)
        {
            var p = new Person
            {
                Fandoms = fandoms ?? new [] {this.universalFandom},
                IsReader = reader,
                IsWriter = writer,
                Id = idGen.GetId()
            };
            this.people.Add(p);
            return p;
        }

        private Fandom universalFandom;

        private Fandom MakeFandom()
        {
            return new Fandom { Id = this.idGen.GetId() };
        }

        private Person MakeReader(ICollection<Fandom> fandom = null)
        {
            return this.MakePerson(fandom, reader: true);
        }

        private Person MakeWriter(ICollection<Fandom> fandom = null)
        {
            return this.MakePerson(fandom, writer: true);
        }

        private void MakeWriters(int n)
        {
            for (var i = 0; i < n; i++)
            {
                this.MakeWriter();
            }
        }

        private MatchResult Match(params Person[] people)
        {
            IEnumerable<Person> matchPeople = people;
            if (!people.Any())
            {
                matchPeople = this.people;
            }
            return new Matcherizer(matchPeople, existingMatches).Matcherize();
        }

        [Test]
        public void PerfectPairingIsMatched()
        {
            var fandom = new [] {MakeFandom()};
            var reader = this.MakePerson(fandom, reader: true);
            var writer = this.MakePerson(fandom, writer: true);

            var matchInfo = new Matcherizer(new[] { reader, writer}, existingMatches).Matcherize();
            var matches = matchInfo.Matches;

            Assert.That(matches.Count(), Is.EqualTo(1), "There should be exactly 1 match");
            var match = matches.Single();
            Assert.That(match.Reader, Is.EqualTo(reader), "The reader of the match should be our reader person");
            Assert.That(match.Writer, Is.EqualTo(writer), "The writer of the match should be our writer person");
            Assert.That(match.Fandom, Is.EqualTo(fandom.Single()), "The fandom of the match should be our fandom");
            Assert.That(matchInfo.UnmatchedPeople, Is.Empty, "There should be no unmatched people");
        }

        [Test]
        public void UnmatchedPeopleAreUnmatched()
        {
            var popularFandom = new [] {MakeFandom()};
            var loneliestFandom = new [] {MakeFandom()};
            var personA = MakePerson(popularFandom, reader: true);
            var personB = MakePerson(popularFandom, writer: true);
            var personC = MakePerson(loneliestFandom, reader: true, writer: true);

            var res = this.Match(personA, personB, personC);

            Assert.That(res.Matches.Count(), Is.EqualTo(1), "There should be 1 match");
            Assert.That(res.UnmatchedPeople.Count(), Is.EqualTo(1), "There should be exactly 1 unmatched person");
            Assert.That(res.UnmatchedPeople.Single(), Is.EqualTo(personC),
                "Person C should be unmatched because no one else is in their fandom");
        }

        [Test]
        public void MultipleMatchPeopleCanMatchMultipleTimes()
        {
            var mx = MakeReader();
            mx.CanMatchMultiple = true;
            MakeWriter();
            MakeWriter();

            var matches = this.Match().Matches;

            Assert.That(matches.Count(m => m.Reader == mx), Is.EqualTo(2), "There should be 2 matches for the multi-match person");
        }

        private void AssertMatch(ICollection<Match> matches, Person a, Person b, string message)
        {
            Assert.That(matches.Any(m => m.Reader == a && m.Writer == b || m.Writer == a && m.Reader == b),
                message);
        }

        private void AssertMatch(ICollection<Match> matches, Person a, string message)
        {
            Assert.That(matches.Any(m => m.Reader == a || m.Writer == a),
                message);
        }

        [Test]
        public void EveryoneGetsOneMatchBeforeWeLookForDoubleMatches()
        {
            var mx = MakeReader();
            mx.CanMatchMultiple = true;
            var single = MakeReader();
            MakeWriter();
            MakeWriter();

            var matches = this.Match().Matches;

            this.AssertMatch(matches, single, "The non-mx person should have a match");
            this.AssertMatch(matches, mx, "The mx person should also have a match");
        }

        [Test]
        public void SingleMatchPeopleAreOnlyMatchedOnce()
        {
            MakeReader();
            MakeWriter();
            MakeWriter();

            var matches = this.Match().Matches;

            Assert.That(matches.Count(), Is.EqualTo(1), "There should only be one match for a single-match person");
        }

        [Test]
        public void MultiplePeopleObeyTheMatchLimit()
        {
            MakeReader().CanMatchMultiple = true;
            MakeWriters(Matcherizer.MAX_MATCHES_PER_PERSON + 1);

            var matches = this.Match().Matches;

            Assert.That(matches.Count, Is.EqualTo(Matcherizer.MAX_MATCHES_PER_PERSON),
                "The maximum match limit should be obeyed");

        }

        [Test]
        public void YouCanOnlyMatchIfYouShareAFandom()
        {
            var fandomA = new [] {MakeFandom()};
            var fandomB = new [] {MakeFandom()};
            MakeReader(fandomA);
            MakeWriter(fandomB);

            var matches = this.Match().Matches;

            Assert.That(matches, Is.Empty, "There should be no matches because the people don't share a fandom");
        }

        [Test]
        public void MultiMatchPeopleAreNotUnmatchedIfTheyHaveOneMatch()
        {
            var fandomA = MakeFandom();
            var fandomB = MakeFandom();
            MakeReader(new [] {fandomA, fandomB}).CanMatchMultiple = true;
            MakeWriter(new [] {fandomA});

            var matchi = this.Match();

            Assert.That(matchi.Matches.Count, Is.EqualTo(1), "There should be exactly one match");
            Assert.That(matchi.UnmatchedPeople, Is.Empty, "There should be no unmatched people because everyone has a match");
        }

        private void Ban(Person reader, Person writer)
        {
            this.existingMatches.Add(new Match { IsBanned = true, Reader = reader, Writer = writer });
        }

        private void Lock(Person reader, Person writer)
        {
            this.existingMatches.Add(new Match { IsLocked = true, Reader = reader, Writer = writer });
        }

        [Test]
        public void BannedMatchesAreNotAllowed()
        {
            var reader = MakeReader();
            var writer = MakeWriter();
            this.Ban(reader, writer);

            var matchi = this.Match();

            Assert.That(matchi.Matches, Is.Empty, "There should be no matches because all of them are banned");
            Assert.That(matchi.UnmatchedPeople.Count, Is.EqualTo(2), "Both people should be unmatched");
            Assert.That(matchi.BannedMatches.Count, Is.EqualTo(1), "The banned match should come through");
        }

        [Test]
        public void LockedMatchesAreForced()
        {
            var fandomA = MakeFandom();
            var fandomB = MakeFandom();
            var r = MakeReader(new[] { fandomA });
            var w = MakeWriter(new[] { fandomB});
            this.Lock(r, w);

            var matchi = this.Match();

            Assert.That(matchi.LockedMatches.Count, Is.EqualTo(1), "There should be a match despite the fandom mismatch because it is locked");
            Assert.That(matchi.UnmatchedPeople, Is.Empty, "There should be no unmatched people");
        }

        [Test]
        public void MultiMatchersWontMatchEachOtherMultiply()
        {
            MakePerson(reader: true, writer: true).CanMatchMultiple = true;
            MakePerson(reader: true, writer: true).CanMatchMultiple = true;

            var matches = this.Match().Matches;

            Assert.That(matches.Count, Is.EqualTo(1), "There should still only be 1 match because there are only 2 people");
        }
    }
}
