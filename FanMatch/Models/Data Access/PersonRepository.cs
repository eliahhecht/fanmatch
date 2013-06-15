using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Data_Access;
using System.Data.Entity;

namespace FanMatch.Models
{
    public interface IPersonRepository : IGenericRepository<Person>, IDisposable
    {

    }

    public class PersonRepository :  GenericCrudRepository<Person>, IPersonRepository
    {
        protected override IDbSet<Person> GetDbSet(FanMatchDb db)
        {
            return db.People;
        }

        protected override void TransferProperties(Person newEntity, Person existing)
        {
            existing.Name = newEntity.Name;
            existing.Fandoms = newEntity.Fandoms;
        }

    }
}