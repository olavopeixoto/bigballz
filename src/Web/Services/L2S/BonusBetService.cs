using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class BonusBetService : IBonusBetService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<BonusBet> GetAll()
        {
            return db.BonusBets.OrderBy(x => x.User).ThenBy(x => x.BonusBetId);
        }

        public IQueryable<BonusBet> GetAll(string userName)
        {
            return db.BonusBets.Where(d => d.User1.UserName == userName);
        }

        public IQueryable<BonusBet> GetAll(int userId)
        {
            return db.BonusBets.Where(d => d.User == userId);
        }

        public BonusBet Get(int bonusBetId)
        {
            return db.BonusBets.SingleOrDefault(d => d.BonusBetId == bonusBetId);
        }

        public void Add(BonusBet bonusBet)
        {
            db.BonusBets.InsertOnSubmit(bonusBet);
        }

        public void Add(System.Collections.Generic.IList<BonusBet> bonusBetList)
        {
            db.BonusBets.InsertAllOnSubmit(bonusBetList);
        }

        public void Delete(BonusBet bonusBet)
        {
            db.BonusBets.DeleteOnSubmit(bonusBet);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }
}
