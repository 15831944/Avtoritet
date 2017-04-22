using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using PushNotification.DataContext;

namespace PushNotification.Repository
{
    public class SettingRepository:ISettingsRepository
    {
        [Inject]
        public AvtoritetEntities Db { get; set; }

        public int GetSettingVersion()
        {
            var settingVersion= Db.SettingUpdate.FirstOrDefault();
            if (settingVersion != null)
            {
                var setVer= settingVersion.SettingVersion;
                return (int) setVer;
            }
            return 0;
        }
    }
}