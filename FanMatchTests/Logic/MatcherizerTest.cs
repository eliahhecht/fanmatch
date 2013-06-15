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

        [SetUp]
        public void Setup()
        {
            this.people = new List<Person>();
        }

        private Person MakePerson(Fandom fandom = null, bool reader = false, bool writer = false)
        {
            var p = new Person
            {
                Fandoms = new[] { fandom ?? this.universalFandom },
                IsReader = reader,
                IsWriter = writer,
                Id = idGen.GetId()
            };
            this.people.Add(p);
            return p;
        }

        private Fandom universalFandom = new Fandom();

        private Person MakeReader(Fandom fandom = null)
        {
            return this.MakePerson(fandom, reader: true);
        }

        private Person MakeWriter(Fandom fandom = null)
        {
            return this.MakePerson(fandom, writer: true);
        }

        private MatchResult Match(params Person[] people)
        {
            IEnumerable<Person> matchPeople = people;
            if (!people.Any())
            {
                matchPeople = this.people;
            }
            return new Matcherizer(matchPeople).Matcherize();
        }

        [Test]
        public void PerfectPairingIsMatched()
        {
            var fandom = new Fandom();
            var reader = this.MakePerson(fandom, reader: true);
            var writer = this.MakePerson(fandom, writer: true);

            var matchInfo = new Matcherizer(new[] { reader, writer}).Matcherize();
            var matches = matchInfo.Matches;

            Assert.That(matches.Count(), Is.EqualTo(1), "There should be exactly 1 match");
            var match = matches.Single();
            Assert.That(match.Reader, Is.EqualTo(reader), "The reader of the match should be our reader person");
            Assert.That(match.Writer, Is.EqualTo(writer), "The writer of the match should be our writer person");
            Assert.That(match.Fandom, Is.EqualTo(fandom), "The fandom of the match should be our fandom");
            Assert.That(matchInfo.UnmatchedPeople, Is.Empty, "There should be no unmatched people");
        }

        [Test]
        public void UnmatchedPeopleAreUnmatched()
        {
            var popularFandom = new Fandom();
            var loneliestFandom = new Fandom();
            var personA = MakePerson(popularFandom, reader: true);
            var personB = MakePerson(popularFandom, writer: true);
            var personC = MakePerson(loneliestFandom, reader: true, writer: true);

            var res = this.Match(personA, personB, personC);

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
        public void SingleMatchPeopleAreOnlyMatchedONce()
        {
            MakeReader();
            MakeWriter();
            MakeWriter();

            var matches = this.Match().Matches;

            Assert.That(matches.Count(), Is.EqualTo(1), "There should only be one match for a single-match person");
        }
    }
}
