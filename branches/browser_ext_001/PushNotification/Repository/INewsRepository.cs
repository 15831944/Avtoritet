using System.Linq;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public interface INewsRepository
    {
        IQueryable<NewsLog> NewsLogs { get; }

        void Create(NewsLog newsLog);

        void Update(NewsLog newsLog);

        void Remove(NewsLog newsLog);
    }
}