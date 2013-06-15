using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FanMatch.Models.Entities;

namespace FanMatch.Models.Data_Access
{
    public interface IGenericRepository<T> where T : class, IId
    {
        void Delete(int id);
        void Create(T entity);
        IList<T> GetAll();
        T GetById(int id);
        void Update(T newEntity);
    }

    public abstract class GenericCrudRepository<T> : IGenericRepository<T>, IDisposable
        where T : class, IId
    {
        protected abstract IDbSet<T> GetDbSet(FanMatchDb db);

        private readonly FanMatchDb db;

        public GenericCrudRepository()
        {
            this.db = new FanMatchDb();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }



        public void Create(T entity)
        {
            var set = GetDbSet(db);
            set.Add(entity);
            db.SaveChanges();
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
            var set = GetDbSet(db);
            var existing = set.Find(id);
            AssertExists(existing);
            set.Remove(existing);
            db.SaveChanges();
        }

        public IList<T> GetAll()
        {
            var set = GetDbSet(db);
            return set.ToList();
        }

        public T GetById(int id)
        {
            return GetDbSet(db).Find(id);
        }

        protected abstract void TransferProperties(T newEntity, T existing);

        public void Update(T newEntity)
        {
            var set = GetDbSet(db);
            var existing = set.Find(newEntity.Id);
            AssertExists(existing);
            db.SaveChanges();
        }
    }
}