using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanMatch.Models.Logic
{
    public class MatchResult
    {
        public ICollection<Match> Matches { get; set; }
        public ICollection<Person> UnmatchedPeople { get; set; }
        public ICollection<Match> LockedMatches { get; set; }
        public ICollection<Match> BannedMatches { get; set; }

        public MatchResult()
        {
            Matches = new List<Match>();
            UnmatchedPeople = new List<Person>();
            LockedMatches = new List<Match>();
            BannedMatches = new List<Match>();
        }
    }
}