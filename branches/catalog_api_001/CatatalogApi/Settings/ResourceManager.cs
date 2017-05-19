using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace CatalogApi.Settings
{
    public static class ResourceManager
    {
        private static readonly string ResourceFolder;

        private static readonly string UpadteFileArchive;

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

        public static string Archive { get { return UpadteFileArchive; } }

        static ResourceManager()
        {
            UpdateDirectory = ConfigurationManager.AppSettings["UpdateDirectory"]; // "Temp"
            ResourceFolder = ConfigurationManager.AppSettings["ResourceDirectory"];
            UpadteFileArchive = ConfigurationManager.AppSettings["UpdateFileArchive"];

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
