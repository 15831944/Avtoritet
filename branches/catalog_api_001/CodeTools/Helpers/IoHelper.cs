using System.IO;
using CodeTools.Extensions;

namespace CodeTools.Helpers
{
    public static class IoHelper
    {
        public static void CopyStream(Stream input, Stream output)
        {
            // TODO: Перенести в константы.
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0) {
                output.Write(buffer, 0, len);
            }
        }

        public static void SaveToFile(this Stream stream, string nameFile)
        {
            using (var fileStream = File.Create(nameFile)) {
                CopyStream(stream, fileStream);
            }
        }

        public static void SaveToFile(string content, string nameFile)
        {
            MemoryStream stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(content));
            SaveToFile(stream, nameFile);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
  {
   var sourceDirectory = new DirectoryInfo(sourceDirName);
 
   if (!sourceDirectory.Exists)
   {
    throw new DirectoryNotFoundException("Source directory does not exist or could not be found: '{0}'.".FormatString(sourceDirName));
   }

   if (!Directory.Exists(destDirName))
   {
    Directory.CreateDirectory(destDirName);
   }

   foreach (var file in sourceDirectory.GetFiles())
   {
    var temppath = Path.Combine(destDirName, file.Name);
    file.CopyTo(temppath, false);
   }

   if (copySubDirs)
   {
    foreach (var subdir in sourceDirectory.GetDirectories())
    {
     var temppath = Path.Combine(destDirName, subdir.Name);
     DirectoryCopy(subdir.FullName, temppath, true);
    }
   }
  }

        public static bool DirectoryClear(string sourceDirName)
        {
            try {
                var sourceDirectory = new DirectoryInfo(sourceDirName);

                foreach (var file in sourceDirectory.GetFiles()) {
                    file.Delete();
                }

                foreach (var subdir in sourceDirectory.GetDirectories()) {
                    DirectoryClear(subdir.FullName);
                }

                var directories = new DirectoryInfo(sourceDirName).GetDirectories();

                foreach (var directory in directories) {
                    directory.Delete(true);
                }
            } catch {
                return false;
            }

            return true;
        }

        public static void CreateDirectoryIfNotExist(string dirPath)
        {
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
        }

        public static string OpenFile(string filePath, out bool exists)
        {
            string str = string.Empty;

            exists = System.IO.File.Exists(filePath);

            if (exists == true)
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        str = reader.ReadToEnd();
                    }
                }
            else
                ;

            return str;
        }

        public static bool DirectoryHasFiles(string sourceDirName)
        {
            return Directory.Exists(sourceDirName) && Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories).Length > 0;
        }
    }
}