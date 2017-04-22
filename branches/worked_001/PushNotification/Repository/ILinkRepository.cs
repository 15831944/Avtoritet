using System.Linq;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public interface ILinkRepository
    {
        IQueryable<Link> Links { get; }

        void Create(Link link);

        void Update(Link link);

        void Remove(Link link);
    }
}