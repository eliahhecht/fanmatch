using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FanMatch.Models.Data_Access
{
    public interface IMatchRepository : IDisposable
    {
        IEnumerable<Match> LoadAll();
        void Lock(int reader, int writer);
        void Ban(int reader, int writer);
        void Clear(int reader, int writer);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly FanMatchDb db;

        public MatchRepository()
        {
            this.db = new FanMatchDb();
        }

        public IEnumerable<Match> LoadAll()
        {
            return this.db.Matches
                .Include(m => m.Reader.Fandoms)
                .Include(m => m.Writer.Fandoms);
        }

        public void Clear(int reader, int writer)
        {
            var match = this.db.Matches.SingleOrDefault(m => m.Reader.Id == reader && m.Writer.Id == writer);
            if (match != null)
            {
                this.db.Matches.Remove(match);
                this.db.SaveChanges();
            }
        }

        private void MakeMatch(int reader, int writer, bool banned = false, bool locked = false)
        {
            this.Clear(reader, writer);
            var match = new Match 
            { 
                Reader = LoadPerson(reader),
                Writer = LoadPerson(writer),
                IsBanned = banned,
                IsLocked = locked
            };
            this.db.Matches.Add(match);
            this.db.SaveChanges();
        }


        public void Ban(int reader, int writer)
        {
            this.MakeMatch(reader, writer, banned: true);
        }

        public void Lock(int reader, int writer)
        {
            this.MakeMatch(reader, writer, locked: true);
        }


        private Person LoadPerson(int id)
        {
            return this.db.People.Find(id);
        }



        public void Dispose()
        {
            this.db.Dispose();
        }

    }
}