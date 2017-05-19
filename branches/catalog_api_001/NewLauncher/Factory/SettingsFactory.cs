namespace NewLauncher.Factory
{
    using CatalogApi.Exceptions;
    using CatalogApi.Settings;
    using Helper;
    using NewLauncher.DataContext;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;

    public class SettingsFactory
    {
        public SettingsFactory(ICategoryEventHandler eventHandler)
        {
        }

        private static void ClearRootDirectory(string root)
        {
            DirectoryInfo info = new DirectoryInfo(root);
            foreach (FileInfo info2 in info.GetFiles())
            {
                info2.Delete();
            }
            foreach (DirectoryInfo info3 in info.GetDirectories())
            {
                info3.Delete(true);
            }
        }

        private void CreateRootDirectory(string root, LauncherSettings settings)
        {
            string path = string.Empty;

            foreach (GroupSet set in settings.Groups)
            {
                foreach (CatalogApi.Settings.GroupBox box in set.GroupBoxs)
                {
                    foreach (CatalogApi.Settings.Brand brand in box.Brands)
                    {
                        try
                        {
                            path = Path.Combine(root, brand.NameAndFolder);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            if (brand.IconPathImg != null)
                            {
                                File.WriteAllBytes(string.Join(@"\", new string[] { path, brand.IconPath }), brand.IconPathImg);
                            }
                            if (brand.IconPath2Img != null)
                            {
                                File.WriteAllBytes(string.Join(@"\", new string[] { path, brand.IconPath2 }), brand.IconPath2Img);
                            }
                            foreach (BrandProvider provider in brand.Providers)
                            {
                                foreach (CatalogApi.Settings.CommandFile file in provider.CommandFiles)
                                {
                                    using (StreamWriter writer = new StreamWriter(string.Join(@"\", new string[] { path, file.FileName })))
                                    {
                                        writer.Write(file.FileContent);
                                    }
                                }
                                foreach (CatalogApi.Settings.ProviderFile file2 in provider.ProviderFiles)
                                {
                                    File.WriteAllBytes(string.Join(@"\", new string[] { path, file2.FileName }), file2.FileContent);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            ErrorLogHelper.AddErrorInLog(string.Format("::CreateRootDirectory (root={0}, brand.Id={1}, brand.NameAndFolder={2}, path={3}) - ..."
                                    , root
                                    , brand.BrandId, brand.NameAndFolder
                                    , path)
                                , string.Format("{0} | {1}", e.Message, e.StackTrace));
                        }
                    }
                }
            }
        }

        public LauncherSettings DownloadSettings(bool force, bool FullLoad = true)
        {
            string nameFileDbSerialize = "launcherDBSettings.dat";
            LauncherSettings graph = new LauncherSettings();
            // режим "force" принажатии кнопки брэнда
            if ((force == true)
                && (File.Exists(nameFileDbSerialize) == true)) {
                File.Delete(nameFileDbSerialize);
            } else
                ;

            if (File.Exists(nameFileDbSerialize) == true) {
            // для режима forse=false - прочитать и вернуть копию БД
                Stream stream = File.OpenRead(nameFileDbSerialize);
                BinaryFormatter formatter = new BinaryFormatter();
                graph = (LauncherSettings)formatter.Deserialize(stream);
                stream.Close();

                return graph;
            } else
                ;

            #region чтение БД, заполнение объектов значениями
            using (AvtoritetEntities entities = new AvtoritetEntities())
            {
                List<NewLauncher.DataContext.Brand> list = (from r in entities.Brand
                    where r.Enable
                    orderby r.NameAndFolder
                    select r).ToList<NewLauncher.DataContext.Brand>();
                List<Provider> list2 = (from r in entities.Provider
                    where r.Enable & FullLoad
                    orderby r.Order
                    select r).ToList<Provider>();
                List<ProviderAccount> list3 = (from t in entities.ProviderAccount
                    where t.Enable & FullLoad
                    select t).ToList<ProviderAccount>();
                List<NewLauncher.DataContext.CommandFile> list4 = (from t in entities.CommandFile
                    where FullLoad
                    select t).ToList<NewLauncher.DataContext.CommandFile>();
                List<NewLauncher.DataContext.ProviderFile> list5 = (from t in entities.ProviderFile
                    where FullLoad
                    select t).ToList<NewLauncher.DataContext.ProviderFile>();
                List<Group> list6 = (from t in entities.Group
                    where t.Enable
                    orderby t.Order
                    select t).ToList<Group>();
                List<NewLauncher.DataContext.GroupBox> list7 = (from r in entities.GroupBox
                    where r.Enable
                    orderby r.Title
                    select r).ToList<NewLauncher.DataContext.GroupBox>();
                foreach (Group group in list6)
                {
                    GroupSet item = new GroupSet {
                        Name = group.Name,
                        Width = group.Width.Value,
                        Height = group.Height.Value,
                        GroupBoxs = new List<CatalogApi.Settings.GroupBox>()
                    };
                    foreach (NewLauncher.DataContext.GroupBox box in list7)
                    {
                        if (box.GroupId == group.GroupId)
                        {
                            CatalogApi.Settings.GroupBox box2 = new CatalogApi.Settings.GroupBox {
                                Left = box.Left.Value,
                                Top = box.Top.Value,
                                Width = box.Width.Value,
                                Height = box.Height.Value,
                                Title = box.Title,
                                VisibleBorder = box.VisibleBorder.Value,
                                Brands = new List<CatalogApi.Settings.Brand>()
                            };
                            foreach (NewLauncher.DataContext.Brand brand in list)
                            {
                                if (brand.GroupBoxId == box.GroupBoxId)
                                {
                                    CatalogApi.Settings.Brand brand2 = new CatalogApi.Settings.Brand {
                                        NameAndFolder = brand.NameAndFolder,
                                        IconPath = brand.IconPath,
                                        IconPath2 = brand.IconPath2,
                                        IconPathImg = brand.IconPathImg,
                                        IconPath2Img = brand.IconPath2Img
                                    };
                                    int? top = brand.Top;
                                    brand2.Top = top.HasValue ? top.GetValueOrDefault() : 0;
                                    top = brand.Left;
                                    brand2.Left = top.HasValue ? top.GetValueOrDefault() : 0;
                                    top = brand.Width;
                                    brand2.Width = top.HasValue ? top.GetValueOrDefault() : 0;
                                    top = brand.Height;
                                    brand2.Height = top.HasValue ? top.GetValueOrDefault() : 0;
                                    brand2.BrandId = brand.BrandId;
                                    brand2.ButtonStyle = brand.ButtonStyle;
                                    brand2.MenuWindow = brand.MenuWindow.Value;
                                    brand2.Providers = new List<BrandProvider>();
                                    foreach (Provider provider in list2)
                                    {
                                        int providerId;
                                        if (provider.BrandId != brand.BrandId)
                                        {
                                            continue;
                                        }
                                        BrandProvider provider2 = new BrandProvider {
                                            Uri = provider.Uri.Trim(),
                                            IconPath = provider.IconPath,
                                            Title = provider.Title,
                                            Commands = provider.commandcontent,
                                            ProviderId = provider.ProviderId,
                                            Login = "",
                                            Password = ""
                                        };
                                        foreach (ProviderAccount account in list3)
                                        {
                                            if (account.ProviderId == provider.ProviderId)
                                            {
                                                provider2.Login = (account != null) ? account.Login.Trim() : string.Empty;
                                                provider2.Password = (account != null) ? account.Password.Trim() : string.Empty;
                                                break;
                                            }
                                        }
                                        provider2.CommandFiles = new List<CatalogApi.Settings.CommandFile>();
                                        foreach (NewLauncher.DataContext.CommandFile file in list4)
                                        {
                                            top = file.ProviderId;
                                            providerId = provider.ProviderId;
                                            if ((top.GetValueOrDefault() == providerId) && top.HasValue)
                                            {
                                                CatalogApi.Settings.CommandFile file2 = new CatalogApi.Settings.CommandFile {
                                                    FileName = file.FileName,
                                                    FileContent = file.FileContent
                                                };
                                                provider2.CommandFiles.Add(file2);
                                            }
                                        }
                                        provider2.ProviderFiles = new List<CatalogApi.Settings.ProviderFile>();
                                        foreach (NewLauncher.DataContext.ProviderFile file3 in list5)
                                        {
                                            top = file3.ProviderId;
                                            providerId = provider.ProviderId;
                                            if ((top.GetValueOrDefault() == providerId) && top.HasValue)
                                            {
                                                CatalogApi.Settings.ProviderFile file4 = new CatalogApi.Settings.ProviderFile {
                                                    FileName = file3.FileName,
                                                    FileExt = file3.FileExt,
                                                    FileSize = file3.FileSize.Value,
                                                    FileContent = file3.FileContent
                                                };
                                                provider2.ProviderFiles.Add(file4);
                                            }
                                        }
                                        brand2.Providers.Add(provider2);
                                    }
                                    box2.Brands.Add(brand2);
                                }
                            }
                            item.GroupBoxs.Add(box2);
                        }
                    }
                    graph.Groups.Add(item);
                }
            }
            #endregion

            if (File.Exists(nameFileDbSerialize) == true) {
            //??? никогда не выполняется
            // , если forse=true файл будет удален
            // , если forse=false произодет возврат значения и выход из функции
                File.Delete(nameFileDbSerialize);
            } else
                ;

            Stream serializationStream = File.Create(nameFileDbSerialize);
            new BinaryFormatter().Serialize(serializationStream, graph);
            serializationStream.Close();

            if (force == true) {
                this.ExtractResourcesToFolder(graph);
            } else
                ;

            return graph;
        }

        private void ExtractResourcesToFolder(LauncherSettings settings)
        {
            try
            {
                string root = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ResourceManager.Root);
                try
                {
                    ClearRootDirectory(root);
                }
                catch (Exception e)
                {
                    ErrorLogHelper.AddErrorInLog(string.Format("::ExtractResourcesToFolder (step=clearRootDirectory) - root={0}..."
                            , root)
                        , string.Format("{0} | {1}", e.Message, e.StackTrace));
                }
                try
                {
                    this.CreateRootDirectory(root, settings);
                }
                catch (Exception e)
                {
                    ErrorLogHelper.AddErrorInLog(string.Format("::ExtractResourcesToFolder (step=createRootDirectory) - root={0}..."
                            , root)
                        , string.Format("{0} | {1}", e.Message, e.StackTrace));
                }
            }
            catch
            {
            }
        }

        public static string GetLoginFromDB(long ProviderId)
        {
            string str = "";
            using (AvtoritetEntities entities = new AvtoritetEntities())
            {
                List<ProviderAccount> list = (from t in entities.ProviderAccount
                    where (t.ProviderId == ProviderId) && t.Enable
                    orderby t.ProviderAccountId
                    select t).ToList<ProviderAccount>();
                foreach (ProviderAccount account in list)
                {
                    return ((account != null) ? account.Login.Trim() : string.Empty);
                }
                return str;
            }
        }

        public static string GetPswFromDB(long ProviderId)
        {
            string str = "";
            using (AvtoritetEntities entities = new AvtoritetEntities())
            {
                List<ProviderAccount> list = (from t in entities.ProviderAccount
                    where (t.ProviderId == ProviderId) && t.Enable
                    orderby t.ProviderAccountId
                    select t).ToList<ProviderAccount>();
                foreach (ProviderAccount account in list)
                {
                    return ((account != null) ? account.Password.Trim() : string.Empty);
                }
                return str;
            }
        }
    }
}

