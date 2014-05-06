using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class TeamService : ITeamService
    {
        readonly BigBallzDataContext _db = new BigBallzDataContext();

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
            _db.SubmitChanges();
        }

    }
}
