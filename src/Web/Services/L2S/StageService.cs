using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class StageService : IStageService
    {
        readonly BigBallzDataContext _db;

        public StageService(BigBallzDataContext context)
        {
            _db = context;
        }

        public IQueryable<Stage> GetAll()
        {
            return _db.Stages;
        }

        public Stage Get(int id)
        {
            return _db.Stages.SingleOrDefault(d => d.StageId == id);
        }

        public void Add(Stage stage)
        {
            _db.Stages.InsertOnSubmit(stage);
        }

        public void Delete(Stage stage)
        {
            _db.Stages.DeleteOnSubmit(stage);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }
    }
}