using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class BonusService : IBonusService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<Bonus> GetAll()
        {
            return db.Bonus.OrderBy(d => d.BonusId);
        }

        public Bonus Get(int id)
        {
            return db.Bonus.SingleOrDefault(d => d.BonusId == id);
        }

        public void Add(Bonus bonus)
        {
            db.Bonus.InsertOnSubmit(bonus);
        }

        public void Delete(Bonus bonus)
        {
            db.Bonus.DeleteOnSubmit(bonus);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
