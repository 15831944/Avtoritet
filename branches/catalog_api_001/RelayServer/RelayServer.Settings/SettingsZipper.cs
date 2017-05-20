using CatalogApi.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RelayServer.Settings
{
	public class SettingsZipper : ISettingsZipper
	{
		public string CreateZipFromSettings(string json)
		{
			string result;
			try
			{
				System.Collections.Generic.IEnumerable<System.IO.DirectoryInfo> availableBrands = from path in ResourceManager.AvailableBrands(json)
				select new System.IO.DirectoryInfo(System.IO.Path.Combine(ResourceManager.Root, path));
				System.IO.DirectoryInfo tempFolder = this.ClearTempFolder();
				System.IO.DirectoryInfo subdirectory = tempFolder.CreateSubdirectory(System.Guid.NewGuid().ToString());
				foreach (System.IO.DirectoryInfo brand in availableBrands)
				{
					SettingsZipper.DirectoryCopy(brand.FullName, System.IO.Path.Combine(subdirectory.FullName, brand.Name), true);
				}
				string archiveFileName = System.IO.Path.Combine(tempFolder.FullName, string.Format("{0}.zip", subdirectory.Name));
				ZipFile.CreateFromDirectory(subdirectory.FullName, archiveFileName);
				result = archiveFileName;
			}
			catch (System.Exception ex)
			{
                CatalogApi.Logging.Exception(ex);
				result = null;
			}
			return result;
		}

		public void UnzipToRoot(string sourceZipFile)
		{
			if (!System.IO.Directory.Exists(ResourceManager.Root))
			{
				throw new System.ArgumentException("Directory not found: '{0}'");
			}
			if (!System.IO.File.Exists(sourceZipFile))
			{
				throw new System.ArgumentException("File not found: '{0}'");
			}
			this.ClearRootFolder();
			ZipFile.ExtractToDirectory(sourceZipFile, ResourceManager.Root);
		}

		private void ClearRootFolder()
		{
			try
			{
				string[] directories = System.IO.Directory.GetDirectories(ResourceManager.Root);
				for (int i = 0; i < directories.Length; i++)
				{
					string directory = directories[i];
					System.IO.Directory.Delete(directory, true);
				}
			}
			catch (System.Exception ex_33)
			{
			}
		}

		private System.IO.DirectoryInfo ClearTempFolder()
		{
			System.IO.DirectoryInfo tempDirectory = new System.IO.DirectoryInfo(System.IO.Path.Combine(ResourceManager.Root, ResourceManager.UpdateDirectory));
			if (!tempDirectory.Exists)
			{
				tempDirectory.Create();
			}
			foreach (System.IO.DirectoryInfo directory in tempDirectory.EnumerateDirectories())
			{
				try
				{
					directory.Delete(true);
				}
				catch (System.Exception ex_50)
				{
				}
			}
			return tempDirectory;
		}

		public void UnzipToUpdate(string sourceZipFile)
		{
            if (!System.IO.Directory.Exists(ResourceManager.UpdateDirectory)) {
                throw new System.ArgumentException("Directory not found: '{0}'");
            } else
                ;

            if (!System.IO.File.Exists(sourceZipFile)) {
                throw new System.ArgumentException("File not found: '{0}'");
            } else
                ;

			ZipFile.ExtractToDirectory(sourceZipFile, ResourceManager.UpdateDirectory);
		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			System.IO.DirectoryInfo sourceDirectory = new System.IO.DirectoryInfo(sourceDirName);
			if (!sourceDirectory.Exists)
			{
				throw new System.IO.DirectoryNotFoundException("Source directory does not exist or could not be found: '{0}'.");
			}
			if (!System.IO.Directory.Exists(destDirName))
			{
				System.IO.Directory.CreateDirectory(destDirName);
			}
			System.IO.FileInfo[] files = sourceDirectory.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				System.IO.FileInfo file = files[i];
				string temppath = System.IO.Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, true);
			}
			if (copySubDirs)
			{
				System.IO.DirectoryInfo[] directories = sourceDirectory.GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					System.IO.DirectoryInfo subdir = directories[i];
					string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
					SettingsZipper.DirectoryCopy(subdir.FullName, temppath, true);
				}
			}
		}
	}
}
