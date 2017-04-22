using System.Linq;
using Ninject;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public class VersionRepository : IVersionRepository
    {
        [Inject]
        public AvtoritetEntities Db { get; set; }

        public IQueryable<VersionLog> Versions
        {
            get { return Db.VersionLog; }
        }

        public void Create(VersionLog versionLog)
        {
            if (versionLog.Id != 0) return;
            Db.VersionLog.Add(versionLog);
            Db.SaveChanges();
        }

        public void Update(VersionLog versionLog)
        {
            Db.SaveChanges();
        }

        public void Remove(VersionLog versionLog)
        {
            var entity = Db.VersionLog.FirstOrDefault(p => p.Id.Equals(versionLog.Id));
            if (entity == null) return;
            Db.VersionLog.Remove(entity);
            Db.SaveChanges();
        }
    }
}