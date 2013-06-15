using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace FanMatch.Models
{


    public class FanMatchDb : DbContext
    {
        public IDbSet<Person> People { get; set; }
        public IDbSet<Match> Matches { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Fandom> Fandoms { get; set; }

     
    }
}