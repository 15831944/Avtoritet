namespace CatalogApi.Settings
{
 public interface ISettingsZipper
 {
  void UnzipToRoot(string sourceZipFile);

  string CreateZipFromSettings(string json);
 }
}