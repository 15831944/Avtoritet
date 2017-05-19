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
   while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
   {
    output.Write(buffer, 0, len);
   }
  }

  public static void SaveToFile(this Stream stream, string file)
  {
   using (var fileStream = File.Create(file))
   {
    CopyStream(stream, fileStream);
   }
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

  public static void DirectoryClear(string sourceDirName)
  {
   var sourceDirectory = new DirectoryInfo(sourceDirName);

   foreach (var file in sourceDirectory.GetFiles())
   {
    file.Delete();
   }

   foreach (var subdir in sourceDirectory.GetDirectories())
   {
    DirectoryClear(subdir.FullName);
   }

   var directories = new DirectoryInfo(sourceDirName).GetDirectories();

   foreach (var directory in directories)
   {
    directory.Delete(true);
   }
  }
 }
}