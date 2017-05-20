using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace CatalogApi.Settings
{
    public static class ResourceManager
    {
        private static readonly string ResourceFolder;

        private static readonly string UpdateFileArchive;

        public static string Root
        {
            get
            {
                return ResourceFolder;
            }
        }

        public static string UpdateDirectory
        {
            get;
        }

        public static string Archive { get { return UpdateFileArchive; } }

        static ResourceManager()
        {
            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("UpdateDirectory") == true)
                UpdateDirectory = ConfigurationManager.AppSettings["UpdateDirectory"]; // "Temp"

            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("ResourceDirectory") == true)
                ResourceFolder = ConfigurationManager.AppSettings["ResourceDirectory"];

            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("UpdateFileArchive") == true)
                UpdateFileArchive = new CatalogApi.Settings.File(ConfigurationManager.AppSettings["UpdateFileArchive"]).Name;

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

            if (!System.IO.Directory.Exists(UpdateDirectory)) {
                System.IO.Directory.CreateDirectory(UpdateDirectory);
            } else
                ;
        }
    }

    public class File
    {
        public enum TYPE { TXT, JSON, RAR, ZIP, DAT, EXE }

        public string Name { get; }

        public TYPE Type { get; }

        public File(string config_value)
        {
            string[] values = config_value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
                if (Enum.IsDefined(typeof(TYPE), values[1].ToUpperInvariant()) == true) {
                    Type = (TYPE)Enum.Parse(typeof(TYPE), values[1].ToUpperInvariant());
                    Name = string.Format("{0}.{1}", values[0], Type.ToString().ToLowerInvariant());
                } else
                // неизвестный тип
                    ;
            else
            // наименование файла указано по неизвестному правилу
                ;
        }
    }
}
