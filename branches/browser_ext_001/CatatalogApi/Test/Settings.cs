using CatalogApi.Settings;

namespace CatalogApi.Test
{
 public class Settings
 {
  public static LauncherSettings CreateTestLauncherSettings()
  {
   var settings = new LauncherSettings();
   settings.Groups.Add(CreateTestGroup("Европа - Азия"));
   return settings;
  }

  private static GroupSet CreateTestGroup(string name)
  {
   var groupSet = new GroupSet { Name = name };

   //groupSet.Brands.Add(new Brand
   //{
   // NameAndFolder = "Infinity",
   // IconPath = "Infinity.ico",
   // Providers = new[]
			//	{
			//		new BrandProvider
			//		{
			//			Title = "Infinity - локально",
			//			IconPath = "Infinity.ico",
			//			Uri = "run.bat"
			//		},
			//		new BrandProvider
			//		{
			//			Title = "Infinity - partslink24",
			//			IconPath = "Infinity.ico",
			//			Uri = "https://www.partslink24.com/partslink24/launchCatalog.do?service=audi_parts"
			//		}
			//	}
   //});

   //groupSet.Brands.Add(new Brand
   //{
   // NameAndFolder = "BMW",
   // IconPath = "BMW.ico",
   // Providers = new[]
			//	{
			//		new BrandProvider
			//		{
			//			Title = "BMW - локально",
			//			Uri = "run.bat",
			//			IconPath = "BMW.ico"
			//		},
			//		new BrandProvider
			//		{
			//			Title = "BWM - partlink24",
			//			Uri = "https://www.partslink24.com/partslink24/launchCatalog.do?service=bmw_parts",
			//			IconPath = "BMW.ico"
			//		}
			//	}
   //});

   //groupSet.Brands.Add(new Brand
   //{
   // NameAndFolder = "Gm",
   // IconPath = "GM.ico",
   // Providers = new[]
			//	{
			//		new BrandProvider
			//		{
			//			Title = "GM - локально",
			//			Uri = "run.bat",
			//			IconPath = "GM.ico"
			//		},
			//		new BrandProvider
			//		{
			//			Title = "GM - онлайн",
			//			Uri = "https://www.partslink24.com/partslink24/launchCatalog.do?service=lancia_parts",
			//			IconPath = "GM.ico"
			//		}
			//	}
   //});

   //groupSet.Brands.Add(new Brand
   //{
   // NameAndFolder = "Nissan",
   // IconPath = "Nissan.ico",
   // Providers = new[]
			//	{
			//		new BrandProvider
			//		{
			//			Title = "Nissan - локально",
			//			Uri = "run.bat",
			//			IconPath = "Nissan.ico"
			//		},
			//		new BrandProvider
			//		{
			//			Title = "Nissan - онлайн",
			//			Uri = "https://www.partslink24.com/partslink24/launchCatalog.do?service=nissan_parts",
			//			IconPath = "Nissan.ico"
			//		}
			//	}
   //});

   //groupSet.Brands.Add(new Brand
   //{
   // NameAndFolder = "Fiat",
   // IconPath = "Fiat.ico",
   // Providers = new[]
			//	{
			//		new BrandProvider
			//		{
			//			Title = "Fiat - локально",
			//			Uri = "run.bat",
			//			IconPath = "Fiat.ico"
			//		},
			//		new BrandProvider
			//		{
			//			Title = "Fiat - онлайн",
			//			Uri = "https://www.partslink24.com/partslink24/launchCatalog.do?service=fiatp_parts",
			//			IconPath = "Fiat.ico"
			//		}
			//	}
   //});

   return groupSet;
  }
 }
}