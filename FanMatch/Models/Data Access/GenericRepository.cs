using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FanMatch.Models.Entities;

namespace FanMatch.Models.Data_Access
{
    public abstract class GenericCrudRepository<T> where T: class, IId
    {
        protected abstract IDbSet<T> GetDbSet(FanMatchDb db);


        private FanMatchDb GetContext()
        {
            return new FanMatchDb();
        }

        public void Create(T entity) {
            using (var db = GetContext())
            {
                var set = GetDbSet(db);
                set.Add(entity);
                db.SaveChanges();
            }
        }

        private void AssertExists(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("This entity doesn't exist");
            }
        }

        public void Delete(int id)
        {
            using (var db = GetContext())
            {
                var set = GetDbSet(db);
                var existing = set.Find(id);
                AssertExists(existing);
                set.Remove(existing);
                db.SaveChanges();
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var db = GetContext())
            {
                var set = GetDbSet(db);
                return set.ToList();
            }
        }

        protected abstract void TransferProperties (T newEntity, T existing);

        public void Update(T newEntity)
        {
            using (var db = GetContext())
            {
                var set = GetDbSet(db);
                var existing = set.Find(newEntity.Id);
                AssertExists(existing);
                db.SaveChanges();
            }
        }
    }
}