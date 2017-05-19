using System.Configuration;
using System.Linq;
using Newtonsoft.Json;

namespace CatalogApi.Settings
{
    public static class ResourceManager
    {
        private static readonly string TempFolder;

        private static readonly string ResourceFolder;

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

        static ResourceManager()
        {
            TempFolder = "Temp"; // ConfigurationManager.AppSettings["TempDirectory"];
            ResourceFolder = ConfigurationManager.AppSettings["ResourceDirectory"];
            Prepare();
        }

        public static System.Collections.Generic.IEnumerable<string> AvailableBrands(string json)
        {
            var launcherSettings = JsonConvert.DeserializeObject<LauncherSettings>(json);

            return launcherSettings.Groups.SelectMany(
                groupSet => groupSet.GroupBoxs
                , (gr, groupbox) => groupbox.Title)
                    .Select(nameAndFolder => string.IsNullOrWhiteSpace(nameAndFolder)
                        ? "not available"
                            : nameAndFolder);
        }

        public static string ResolveBrandImage(Brand brand)
        {
            return System.IO.Path.Combine(Root, brand.NameAndFolder, brand.IconPath2);
        }

        public static string ResolveBrandProviderImage(Brand brand, BrandProvider provider)
        {
            return System.IO.Path.Combine(Root, brand.NameAndFolder, provider.IconPath);
        }

        public static string ResolveExecutingFile(BrandProvider provider)
        {
            return System.IO.Path.Combine(Root, provider.Uri);
        }

        private static void Prepare()
        {
            if (!System.IO.Directory.Exists(Root)) {
                System.IO.Directory.CreateDirectory(Root);
            } else
                ;

            if (!System.IO.Directory.Exists(Temp)) {
                System.IO.Directory.CreateDirectory(Temp);
            } else
                ;
        }
    }
}
