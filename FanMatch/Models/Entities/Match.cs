using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanMatch.Models
{
    public class Match
    {
        public int Id { get; set; }
        public virtual Person Writer { get; set; }
        public virtual Person Reader { get; set; }
        public virtual Project Project { get; set; }
        public virtual Fandom Fandom { get; set; }
        /// <summary>
        /// Is this match not allowed?
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// Is this match required?
        /// </summary>
        public bool IsLocked { get; set; }
    }
}