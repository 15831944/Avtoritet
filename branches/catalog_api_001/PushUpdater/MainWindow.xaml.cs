using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace PushUpdater
{
    public partial class MainWindow
    {
        private const string ZipName = "temp.zip";
        private const string SourceDirName = "Temp";

        public MainWindow()
        {
            try
            {

                InitializeComponent();
   
                new Thread(() =>
                {
                    try
                    {
                        CheckTempFolderForUpdate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " || " + ex.StackTrace);
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Dispatcher.BeginInvoke(new Action(() => Application.Current.Shutdown()));
            }
        }

        private void CheckTempFolderForUpdate()
        {
            Thread.Sleep(3000);

            KillProcessIfExist("NewLauncher");

            Thread.Sleep(3000);

            if (DirectoryHasFiles(SourceDirName) == true)
            {
                DirectoryCopy(SourceDirName, string.Empty, true);

                //TODO: Проверить ход работы после Exception

                if (DirectoryClear(SourceDirName) == true)
                {
                    if (File.Exists(ZipName)) {
                        File.Delete(ZipName);
                    } else
                        ;

                    if (CanStartProcessAsAdministrator() == false)
                        return;
                    else
                        ;
                } else {
                // не удалось очистить каталог
                    if (File.Exists(ZipName)) {
                        File.Delete(ZipName);
                    } else
                        ;

                    if (CanStartProcessAsAdministrator() == false)
                        return;
                    else
                        ;
                }
            }

            Dispatcher.BeginInvoke(new Action(() => Application.Current.Shutdown()));
        }

        private static bool CanStartProcessAsAdministrator()
        {
            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = "NewLauncher.exe",
                Verb = "runas"
            };

            try
            {
                Process.Start(processStartInfo);
            }
            catch
            {
            // The user refused the elevation.
            // Do nothing and return directly ...
                return false;
            }

            return true;
        }

        private static void KillProcessIfExist(string processName)
        {
            foreach (var process in Process.GetProcessesByName(processName))
                process.Kill();
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var sourceDirectory = new DirectoryInfo(sourceDirName);

            if (sourceDirectory.Exists == false)
            {
                throw new DirectoryNotFoundException(
                    string.Format("Source directory does not exist or could not be found: '{0}'."
                        , sourceDirName)
                );
            }

            if ((string.IsNullOrEmpty(destDirName) == false)
                && (Directory.Exists(destDirName) == false))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (var file in sourceDirectory.GetFiles())
            {
                var temppath = Path.Combine(string.IsNullOrEmpty(destDirName) ? string.Empty : destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (!copySubDirs)
                return;

            foreach (var subdir in sourceDirectory.GetDirectories())
            {
                var temppath = Path.Combine(string.IsNullOrEmpty(destDirName) ? string.Empty : destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, true);
            }
        }

        private static bool DirectoryClear(string sourceDirName)
        {
            try
            {
                var sourceDirectory = new DirectoryInfo(sourceDirName);

                foreach (var file in sourceDirectory.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    file.Delete();
                }

                foreach (var subdir in sourceDirectory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Temp"), SearchOption.AllDirectories))
                {
                    subdir.Delete();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool DirectoryHasFiles(string sourceDirName)
        {
            return Directory.Exists(sourceDirName) && Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories).Length > 0;
        }
    }
}
