using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class TeamService : ITeamService
    {
        readonly BigBallzDataContext _db;
        private readonly ICache _cache;

        public TeamService(BigBallzDataContext context, ICache cache)
        {
            _db = context;
            _cache = cache;
        }

        public IQueryable<Team> GetAll()
        {
            return _db.Teams.OrderBy(d => d.Name);
        }

        public IQueryable<Team> GetAll(int groupId)
        {
            return _db.Teams.OrderBy(d => d.Name).Where(d => d.GroupId == groupId);
        }

        public Team Get(string id)
        {
            return _db.Teams.SingleOrDefault(d => d.TeamId == id);
        }

        public void Add(Team team)
        {
            _db.Teams.InsertOnSubmit(team);
        }

        public void Delete(Team team)
        {
            _db.Teams.DeleteOnSubmit(team);
        }

        public void Save()
        {
            _cache.Clear();
            _db.SubmitChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
