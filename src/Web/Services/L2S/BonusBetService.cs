using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class BonusBetService : IBonusBetService
    {
        readonly BigBallzDataContext _db;

        public BonusBetService(BigBallzDataContext context)
        {
            _db = context;
        }

        public IQueryable<BonusBet> GetAll()
        {
            return _db.BonusBets.OrderBy(x => x.User).ThenBy(x => x.BonusBetId);
        }

        public IQueryable<BonusBet> GetAll(string userName)
        {
            return _db.BonusBets.Where(d => d.User1.UserName == userName);
        }

        public IQueryable<BonusBet> GetAll(int userId)
        {
            return _db.BonusBets.Where(d => d.User == userId);
        }

        public BonusBet Get(int bonusBetId)
        {
            return _db.BonusBets.SingleOrDefault(d => d.BonusBetId == bonusBetId);
        }

        public void Add(BonusBet bonusBet)
        {
            _db.BonusBets.InsertOnSubmit(bonusBet);
        }

        public void Add(System.Collections.Generic.IList<BonusBet> bonusBetList)
        {
            _db.BonusBets.InsertAllOnSubmit(bonusBetList);
        }

        public void Delete(BonusBet bonusBet)
        {
            _db.BonusBets.DeleteOnSubmit(bonusBet);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }

    }
}
