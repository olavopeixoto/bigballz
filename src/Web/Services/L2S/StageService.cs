using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class StageService : IStageService
    {
        readonly BigBallzDataContext _db;
        private readonly ICache _cache;

        public StageService(BigBallzDataContext context, ICache cache)
        {
            _db = context;
            _cache = cache;
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
            _db.Stages.Add(stage);
        }

        public void Delete(Stage stage)
        {
            _db.Stages.Remove(stage);
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