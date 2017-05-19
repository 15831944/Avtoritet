using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PushNotification.Repository
{
    public interface ISettingsRepository
    {
        int GetSettingVersion();
    }
}