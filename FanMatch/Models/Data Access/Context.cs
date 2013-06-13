using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FanMatch.Models
{
    public class FanMatchDb : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Fandom> Fandoms { get; set; }
    }
}