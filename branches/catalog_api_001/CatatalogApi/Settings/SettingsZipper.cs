using System;
using System.IO;
using System.Linq;
using CatalogApi.Exceptions;
using CodeTools;
using CodeTools.Extensions;
using CodeTools.Helpers;

namespace CatalogApi.Settings
{
    // TODO: Только при старте сервера!
    public class SettingsZipper : ISettingsZipper
    {
        private const string TempPath = "temp";
        private readonly object sync = new object();
        private readonly ICategoryEventHandler eventHandler;

        public SettingsZipper(ICategoryEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        public string CreateZipFromSettings(string json)
        {
            try
            {
                // TODO: Что делать, если папки, указанные в настройках, отсутствуют фактически?
                var availableBrands = ResourceManager.AvailableBrands(json).Select(path => new DirectoryInfo(Path.Combine(ResourceManager.Root, path)));

                var tempFolder = ClearTempFolder();

                var subdirectory = tempFolder.CreateSubdirectory(Guid.NewGuid().ToString());

                foreach (var brand in availableBrands)
                {
                    IoHelper.DirectoryCopy(brand.FullName, Path.Combine(subdirectory.FullName, brand.Name), true);
                }

                var archiveFileName = Path.Combine(tempFolder.FullName, string.Format("{0}.zip", subdirectory.Name));
                ZipFile.CreateFromDirectory(subdirectory.FullName, archiveFileName);

                return archiveFileName;
            }
            catch (Exception ex)
            {
                eventHandler.ProcessException(ex);
                return null;
            }
        }

        public void UnzipToRoot(string sourceZipFile)
        {
            Guard.CheckContainsText(sourceZipFile, "sourceZipFile");

            if (!Directory.Exists(ResourceManager.Root))
            {
                throw new ArgumentException("Directory not found: '{0}'".FormatString(ResourceManager.Root));
            }

            if (!File.Exists(sourceZipFile))
            {
                throw new ArgumentException("File not found: '{0}'".FormatString(sourceZipFile));
            }

            ClearRootFolder();

            ZipFile.ExtractToDirectory(sourceZipFile, ResourceManager.Root);
        }

        private void ClearRootFolder()
        {
            try
            {
                foreach (var directory in Directory.GetDirectories(ResourceManager.Root))
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
            eventHandler.ProcessException(ex);
            }
        }

        private DirectoryInfo ClearTempFolder()
        {
            var tempDirectory = new DirectoryInfo(Path.Combine(ResourceManager.Root, TempPath));

            if (!tempDirectory.Exists)
            {
                tempDirectory.Create();
            }

            foreach (var directory in tempDirectory.EnumerateDirectories())
            {
                // TODO: Обернуть в безопасный extension.
                try
                {
                    directory.Delete(true);
                }
                catch (Exception ex)
                {
                    eventHandler.ProcessException(ex);
                }
            }

            return tempDirectory;
        }

        //Update Folder

        public void UnzipToUpdate(string sourceZipFile)
        {
            Guard.CheckContainsText(sourceZipFile, "sourceZipFile");

            if (!Directory.Exists("Temp"))
            {
                throw new ArgumentException("Directory not found: '{0}'".FormatString("Temp"));
            }

            if (!File.Exists(sourceZipFile))
            {
                throw new ArgumentException("File not found: '{0}'".FormatString(sourceZipFile));
            }

            ZipFile.ExtractToDirectory(sourceZipFile, "Temp");
        }
    }
}