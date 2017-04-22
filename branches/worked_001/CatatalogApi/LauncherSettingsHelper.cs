using CatalogApi.Settings;
using Newtonsoft.Json;

namespace CatalogApi
{
 public static class LauncherSettingsHelper
 {
  public static string ToJsonFormatted(this LauncherSettings settings)
  {
   return JsonConvert.SerializeObject(settings, Formatting.Indented);
  }

  public static string ToJson(this LauncherSettings settings)
  {
   return JsonConvert.SerializeObject(settings);
  }

  public static T FromString<T>(string json)
  {
   return JsonConvert.DeserializeObject<T>(json);
  }
 }
}