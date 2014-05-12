using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class BonusService : IBonusService
    {
        readonly BigBallzDataContext _db;

        public BonusService(BigBallzDataContext context)
        {
            _db = context;
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
            _db.Bonus.InsertOnSubmit(bonus);
        }

        public void Delete(Bonus bonus)
        {
            _db.Bonus.DeleteOnSubmit(bonus);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }
    }
}
