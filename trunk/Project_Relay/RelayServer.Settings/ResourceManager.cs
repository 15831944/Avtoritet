using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace RelayServer.Settings
{
	public static class ResourceManager
	{
		private static readonly string TempFolder;

		private static readonly string ResourceFolder;

		public static string Root
		{
			get
			{
				return ResourceManager.ResourceFolder;
			}
		}

		public static string Temp
		{
			get
			{
				return ResourceManager.TempFolder;
			}
		}

		static ResourceManager()
		{
			ResourceManager.TempFolder = "Temp";
			ResourceManager.ResourceFolder = ConfigurationManager.AppSettings["ResourceDirectory"];
			ResourceManager.Prepare();
		}

		public static System.Collections.Generic.IEnumerable<string> AvailableBrands(string json)
		{
			LauncherSettings launcherSettings = JsonConvert.DeserializeObject<LauncherSettings>(json);
			return from nameAndFolder in launcherSettings.Groups.SelectMany((GroupSet groupSet) => groupSet.GroupBoxs.SelectMany((GroupBox t) => t.Brands, (GroupBox l, Brand r) => r.NameAndFolder))
			select string.IsNullOrWhiteSpace(nameAndFolder) ? "not available" : nameAndFolder;
		}

		public static string ResolveBrandImage(Brand brand)
		{
			return System.IO.Path.Combine(ResourceManager.Root, brand.NameAndFolder, brand.IconPath2);
		}

		public static string ResolveBrandProviderImage(Brand brand, BrandProvider provider)
		{
			return System.IO.Path.Combine(ResourceManager.Root, brand.NameAndFolder, provider.IconPath);
		}

		public static string ResolveExecutingFile(BrandProvider provider)
		{
			return System.IO.Path.Combine(ResourceManager.Root, provider.Uri);
		}

		private static void Prepare()
		{
			if (!System.IO.Directory.Exists(ResourceManager.Root))
			{
				System.IO.Directory.CreateDirectory(ResourceManager.Root);
			}
			if (!System.IO.Directory.Exists(ResourceManager.Temp))
			{
				System.IO.Directory.CreateDirectory(ResourceManager.Temp);
			}
		}
	}
}
