using FanMatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanMatch.ViewModels
{
    public class FandomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<string> People { get; set; }

        public FandomViewModel()
        {
            this.People = new List<string>();
        }

        public FandomViewModel(Fandom fandom)
        {
            this.Id = fandom.Id;
            this.Name = fandom.Name;
            this.People = fandom.People.Select(p => p.Name).ToList();
        }
    }
}