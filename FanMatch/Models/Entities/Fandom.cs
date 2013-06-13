using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace FanMatch.Models
{
    public class Fandom : IId
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}