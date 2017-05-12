using Ionic.Zip;
using System;
using System.Text;

namespace RelayServer.Settings
{
	public static class ZipFile
	{
		public static void ExtractToDirectory(string sourceZipFile, string targetFolder)
		{
			Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(sourceZipFile);
			zipFile.ExtractAll(targetFolder, ExtractExistingFileAction.OverwriteSilently);
		}

		public static void CreateFromDirectory(string sourceFolder, string archiveFileName)
		{
			using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8))
			{
				zipFile.AddDirectory(sourceFolder);
				zipFile.Save(archiveFileName);
			}
		}
	}
}
