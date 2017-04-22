using System.Linq;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public interface IVersionRepository
    {
        IQueryable<VersionLog> Versions { get; }

        void Create(VersionLog versionLog);

        void Update(VersionLog versionLog);

        void Remove(VersionLog versionLog);
    }
}