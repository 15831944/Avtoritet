using System;
using System.Linq;
using Ninject;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public class NewsRepository : INewsRepository
    {
        [Inject]
        public AvtoritetEntities Db { get; set; }

        public IQueryable<NewsLog> NewsLogs
        {
            get { return Db.NewsLog; }
        }

        public void Create(NewsLog newsLog)
        {
            newsLog.Id = Guid.NewGuid();
            Db.NewsLog.Add(newsLog);
            Db.SaveChanges();
        }

        public void Update(NewsLog newsLog)
        {
            Db.SaveChanges();
        }

        public void Remove(NewsLog newsLog)
        {
            var entity = Db.NewsLog.FirstOrDefault(x => x.Id.Equals(newsLog.Id));
            if (entity == null) return;
            Db.SaveChanges();
        }
    }
}