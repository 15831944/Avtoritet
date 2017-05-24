using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Xml;

namespace CatalogApi.Settings
{
    public static class ResourceManager
    {
        private static readonly string ResourceFolder;

        private static readonly string UpdateFileArchive;

        public static CatalogConstants Catalogs { get; }

        public static UrlConstants Urls { get; }

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
            //XmlDocument xmlDoc;
            string json = string.Empty;

            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("UpdateDirectory") == true)
                UpdateDirectory = ConfigurationManager.AppSettings["UpdateDirectory"]; // "Temp"

            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("ResourceDirectory") == true)
                ResourceFolder = ConfigurationManager.AppSettings["ResourceDirectory"];

            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("UpdateFileArchive") == true)
                UpdateFileArchive = new CatalogApi.Settings.File(ConfigurationManager.AppSettings["UpdateFileArchive"]).Name;

            //xmlDoc = new XmlDocument();
            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("CatalogConstants") == true) {
                //xmlDoc.Load(new CatalogApi.Settings.File(ConfigurationManager.AppSettings["CatalogConstants"]).Name);
                json = CodeTools.Helpers.IoHelper.OpenFile(new CatalogApi.Settings.File(ConfigurationManager.AppSettings["CatalogConstants"]).Name);
            } else
                ;
            //Catalogs = new CatalogConstants(xmlDoc);
            Catalogs = new CatalogConstants(json);

            //xmlDoc = new XmlDocument();
            json = string.Empty;
            if (ConfigurationManager.AppSettings.Keys.Cast<string>().Contains("UrlConstants") == true) {
                //xmlDoc.Load(new CatalogApi.Settings.File(ConfigurationManager.AppSettings["UrlConstants"]).Name);
                json = CodeTools.Helpers.IoHelper.OpenFile(new CatalogApi.Settings.File(ConfigurationManager.AppSettings["UrlConstants"]).Name);
            } else
                ;
            //Urls = new UrlConstants(xmlDoc);
            Urls = new UrlConstants(json);

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

        public string NameWithoutExt { get; }

        public TYPE Type { get; }

        public File(string config_value)
        {
            string[] values = config_value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
                if (Enum.IsDefined(typeof(TYPE), values[1].ToUpperInvariant()) == true) {
                    Type = (TYPE)Enum.Parse(typeof(TYPE), values[1].ToUpperInvariant());
                    NameWithoutExt = string.Format("{0}", values[0]);
                    Name = string.Format("{0}.{1}", NameWithoutExt, Type.ToString().ToLowerInvariant());
                } else
                // неизвестный тип
                    ;
            else
            // наименование файла указано по неизвестному правилу
                ;
        }
    }
}
