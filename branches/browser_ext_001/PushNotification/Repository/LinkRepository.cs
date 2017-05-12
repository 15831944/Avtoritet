using System;
using System.Linq;
using Ninject;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public class LinkRepository : ILinkRepository
    {
        [Inject]
        public AvtoritetEntities Db { get; set; }

        public IQueryable<Link> Links
        {
            get { return Db.Link; }
        }

        public void Create(Link link)
        {
            throw new NotImplementedException();
        }

        public void Update(Link link)
        {
            throw new NotImplementedException();
        }

        public void Remove(Link link)
        {
            throw new NotImplementedException();
        }
    }
}