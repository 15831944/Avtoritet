using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace CatalogApi.Settings
{
    [Serializable]
 public class LauncherSettings
 {
  public const int ColumnsNumber = 3;

  public LauncherSettings()
  {
   Groups = new List<GroupSet>();
  }

  public IList<GroupSet> Groups
  {
   get;
   private set;
  }

  public int Rows
  {
   get
   {
    if (Groups.Count == 0)
    {
     return 0;
    }

    return Groups.Max(group => group.GroupBoxs.Count) / ColumnsNumber;
   }
  }
 }
}