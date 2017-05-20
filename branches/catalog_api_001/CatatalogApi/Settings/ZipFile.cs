using System;
using System.Diagnostics;
using System.Text;
using Ionic.Zip;

namespace CatalogApi.Settings
{
    public static class ZipFile
    {
        public static void ExtractToDirectory(string sourceZipFile, string targetFolder)
        {
            try
            {
                var zipFile = new Ionic.Zip.ZipFile(sourceZipFile);
                zipFile.ExtractAll(targetFolder, ExtractExistingFileAction.OverwriteSilently);
            }
            catch (Exception ex)
            {
                Logging.Exception(ex, true);
            }  
        }

        public static void CreateFromDirectory(string sourceFolder, string archiveFileName)
        {
            using (var zipFile = new Ionic.Zip.ZipFile(Encoding.UTF8))
            {
                zipFile.AddDirectory(sourceFolder);
                zipFile.Save(archiveFileName);
            }
        }
    }
}