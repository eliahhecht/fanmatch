using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Data_Access;

namespace FanMatch.Models
{
    public interface IFandomRepository : IGenericRepository<Fandom>, IDisposable
    {
        Fandom GetByName(string name);
    }
    public class FandomRepository : GenericCrudRepository<Fandom>, IFandomRepository
    {
        protected override System.Data.Entity.IDbSet<Fandom> GetDbSet(FanMatchDb db)
        {
            return db.Fandoms;
        }

        protected override void TransferProperties(Fandom newEntity, Fandom existing)
        {
            existing.Name = newEntity.Name;
        }

        public Fandom GetByName(string name)
        {
            return this.GetDbSet(this.db).FirstOrDefault(f => f.Name == name);
        }

    }
}