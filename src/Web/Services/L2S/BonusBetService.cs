using System.Collections.Generic;
using System.Linq;

namespace BigBallz.Services.L2S
{
    public class BonusBetService : IBonusBetService
    {
        readonly BigBallzDataContext _db;

        public BonusBetService(BigBallzDataContext context)
        {
            _db = context;

            //var options = new DataLoadOptions();
            //options.LoadWith<BonusBet>(x => x.Bonus11);
            //options.LoadWith<BonusBet>(x => x.Team1);

            //_db.LoadOptions = options;
        }

        public IEnumerable<BonusBet> GetAll()
        {
            return _db.BonusBets.OrderBy(x => x.User).ThenBy(x => x.BonusBetId).ToList();
        }

        public IEnumerable<BonusBet> GetAll(string userName)
        {
            return _db.BonusBets.Where(d => d.User1.UserName == userName).ToList();
        }

        public IEnumerable<BonusBet> GetAll(int userId)
        {
            return _db.BonusBets.Where(d => d.User == userId).ToList();
        }

        public BonusBet Get(int bonusBetId)
        {
            return _db.BonusBets.SingleOrDefault(d => d.BonusBetId == bonusBetId);
        }

        public void Add(BonusBet bonusBet)
        {
            _db.BonusBets.Add(bonusBet);
        }

        public void Add(System.Collections.Generic.IList<BonusBet> bonusBetList)
        {
            _db.BonusBets.AddRange(bonusBetList);
        }

        public void Delete(BonusBet bonusBetBet)
        {
            _db.BonusBets.Remove(bonusBetBet);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}