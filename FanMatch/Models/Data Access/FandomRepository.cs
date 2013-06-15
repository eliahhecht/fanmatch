using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanMatch.Models.Data_Access;

namespace FanMatch.Models
{
    public interface IFandomRepository
    {
        IEnumerable<Fandom> GetAll();
        void Create(Fandom fandom);
        void Delete(int id);
        void Update(Fandom fandom);
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

    }
}