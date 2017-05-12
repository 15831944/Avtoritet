using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CatalogApi.Settings
{
 public static class ResourceManager
 {
  private static readonly string TempFolder;
  private static readonly string ResourceFolder;

  static ResourceManager()
  {
   TempFolder = "Temp"; // ConfigurationManager.AppSettings["TempDirectory"];
   ResourceFolder = ConfigurationManager.AppSettings["ResourceDirectory"];
   Prepare();
  }

  public static IEnumerable<string> AvailableBrands(string json)
  {
   var launcherSettings = JsonConvert.DeserializeObject<LauncherSettings>(json);

   return launcherSettings.Groups.SelectMany(groupSet => groupSet.GroupBoxs, (gr, groupbox) => groupbox.Title).Select(nameAndFolder => string.IsNullOrWhiteSpace(nameAndFolder) ? "not available" : nameAndFolder);
  }

  public static string Root
  {
   get
   {
    return ResourceFolder;
   }
  }

  public static string Temp
  {
   get
   {
    return TempFolder;
   }
  }

  public static string ResolveBrandImage(Brand brand)
  {
   return Path.Combine(Root, brand.NameAndFolder, brand.IconPath2);
  }

  public static string ResolveBrandProviderImage(Brand brand, BrandProvider provider)
  {
   return Path.Combine(Root, brand.NameAndFolder, provider.IconPath);
  }

  public static string ResolveExecutingFile(BrandProvider provider)
  {
   return Path.Combine(Root, provider.Uri);
  }

  private static void Prepare()
  {
   if (!Directory.Exists(Root))
   {
    Directory.CreateDirectory(Root);
   }

   if (!Directory.Exists(Temp))
   {
    Directory.CreateDirectory(Temp);
   }
  }
 }
}
