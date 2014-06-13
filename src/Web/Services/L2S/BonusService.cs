using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class BonusService : IBonusService
    {
        readonly BigBallzDataContext _db;
        private readonly ICache _cache;

        public BonusService(BigBallzDataContext context, ICache cache)
        {
            _db = context;
            _cache = cache;
        }

        public IQueryable<Bonus> GetAll()
        {
            return _db.Bonus.OrderBy(d => d.BonusId);
        }

        public Bonus Get(int id)
        {
            return _db.Bonus.SingleOrDefault(d => d.BonusId == id);
        }

        public void Add(Bonus bonus)
        {
            _db.Bonus.Add(bonus);
        }

        public void Delete(Bonus bonus)
        {
            _db.Bonus.Remove(bonus);
        }

        public void Save()
        {
            _cache.Clear();
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
