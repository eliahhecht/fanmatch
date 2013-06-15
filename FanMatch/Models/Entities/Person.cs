using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace FanMatch.Models
{
    public class Person : IId
    {
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Fandom> Fandoms { get; set; }

        public bool IsReader { get; set; }
        public bool IsWriter { get; set; }

        public bool CanMatchMultiple { get; set; }

        public bool Complements(Person other)
        {
            return (IsReader && other.IsWriter)
                || (IsWriter && other.IsReader);
        }
    }
}