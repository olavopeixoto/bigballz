using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class StageService : IStageService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<Stage> GetAll()
        {
            return db.Stages;
        }

        public Stage Get(int id)
        {
            return db.Stages.SingleOrDefault(d => d.StageId == id);
        }

        public void Add(Stage stage)
        {
            db.Stages.InsertOnSubmit(stage);
        }

        public void Delete(Stage stage)
        {
            db.Stages.DeleteOnSubmit(stage);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}