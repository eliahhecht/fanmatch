using FanMatch.Models;
using FanMatch.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanMatch.ViewModels
{
    public class MatchResultViewModel
    {
        public ICollection<MatchViewModel> Matches { get; set; }
        public ICollection<MatchViewModel> Locked { get; set; }
        public ICollection<MatchViewModel> Banned { get; set; }

        public ICollection<Person> UnmatchedPeople { get; set; }

        public MatchResultViewModel(MatchResult res)
        {
            this.Matches = res.Matches.Select(m => new MatchViewModel(m)).ToList();
            this.Locked = res.LockedMatches.Select(m => new MatchViewModel(m)).ToList();
            this.Banned = res.BannedMatches.Select(m => new MatchViewModel(m)).ToList();

            this.UnmatchedPeople = res.UnmatchedPeople.ToList();
        }
    }

    public class MatchViewModel
    {
        public int ReaderId {get;set;}
        public int WriterId {get;set;}
        public string ReaderName {get;set;}
        public string WriterName {get;set;}
        public string FandomName {get;set;}

        public Person Reader { get; set; }
        public Person Writer { get; set; }

        public MatchViewModel(Match m)
        {
            this.Writer = m.Writer;
            this.Reader = m.Reader;
            this.ReaderId = m.Reader.Id;
            this.ReaderName = m.Reader.Name;
            this.WriterId = m.Writer.Id;
            this.WriterName = m.Writer.Name;
            this.FandomName = m.Fandom == null ? String.Empty : m.Fandom.Name;
        }
    }
}