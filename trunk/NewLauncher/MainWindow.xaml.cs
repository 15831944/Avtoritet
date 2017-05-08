using CatalogApi;
using CatalogApi.Settings;
using CodeTools.Helpers;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using Microsoft.Win32;
using NewLauncher.Builder;
using NewLauncher.DataContext;
using NewLauncher.Entities;
using NewLauncher.Factory;
using NewLauncher.Helper;
using NewLauncher.Interop;
using NewLauncher.Manager;
using NewLauncher.ServiceReference;
using NewLauncher.View;
using NewLauncher.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Brand = CatalogApi.Settings.Brand;

namespace NewLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private BrandLauncherView currentBrandLauncher;
        private static readonly CategoryEventHandler categoryEventHandler = new CategoryEventHandler("launcher");
        public bool DownFlag;
        private static bool HaveNewUpdate = false;
        private bool isNormalShutdownMode;
        private static LauncherSettings launcherSettings;
        public bool LeftFlag;
        public double LeftLength;
        private ObservableCollection<NewsModel> news = new ObservableCollection<NewsModel>();
        public bool RightFlag;
        private const int SwRestore = 9;
        private readonly ObservableCollection<TabViewModel> tabViewCollection = new ObservableCollection<TabViewModel>();
        private NewLauncher.Interop.SystemTime time;
        public bool TopFlag;
        public double TopLength;

        Dictionary<int, BrandLauncherView> _dictBrandLauncherView;
        Dictionary<long, BrowserLauncherView> _dictBrowserLauncherView;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            try
            {
                _dictBrandLauncherView = new Dictionary<int, BrandLauncherView>();
                _dictBrowserLauncherView = new Dictionary<long, BrowserLauncherView>();

                EventNewBrandLauncherView += new Action<SystemTime, CatalogApi.Settings.Brand>(newBrandLauncherView);

                this.InitializeComponent();
                base.DataContext = this.News;
                this.NewsBox.ItemsSource = this.News;
                this.InitializeSettings();
                this.StartNewEventSession();
                this.CheckForUpdateAndStartProcess();
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("Запуск приложения - MainWindow()",
                    exception.Message + " | " + exception.StackTrace);
                MessageBox.Show(exception.Message + " | " + exception.StackTrace);

                if (!(Application.Current == null))
                    Application.Current.Shutdown(0);
                else
                    ;
            }
        }

        private void BuildWindow()
        {
            this.tabViewCollection.Clear();
            Version version = new Version(JsonConvert.DeserializeObject<VersionEntity>(FileHelper.OpenFile("Version.json")).Version);
            RequestHelper.Client.LogConnection(Environment.MachineName, version.ToString());
            base.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (GroupSet set in launcherSettings.Groups)
                {
                    ObservableCollection<TabGroupModel> observables = new ObservableCollection<TabGroupModel>();
                    foreach (CatalogApi.Settings.GroupBox box in set.GroupBoxs)
                    {
                        ObservableCollection<TabItemModel> observables2 = new ObservableCollection<TabItemModel>();
                        foreach (CatalogApi.Settings.Brand brand in from x in box.Brands
                                                                    orderby x.NameAndFolder
                                                                    select x)
                        {
                            TabItemModel model = new TabItemModel
                            {
                                Group = set.Name,
                                GroupBox = box.Title,
                                BrandName = brand.NameAndFolder,
                                BrandIcon = LoadImage(brand.IconPathImg),
                                ClickCommand = new RelayCommand<CatalogApi.Settings.Brand>(button_onClick),
                                Top = brand.Top,
                                Left = brand.Left,
                                Height = brand.Height,
                                Width = brand.Width,
                                BrandId = brand.BrandId,     
                                Brand = brand,
                                ButtonStyle = brand.ButtonStyle
                            };
                            observables2.Add(model);
                        }
                        TabGroupModel model2 = new TabGroupModel
                        {
                            TabItemCollection = observables2,
                            Title = box.VisibleBorder ? box.Title : string.Empty,
                            Top = box.Top,
                            Left = box.Left,
                            Width = box.Width,
                            Height = box.Height,
                            VisualBorder = box.VisibleBorder ? 2 : 0
                        };
                        observables.Add(model2);
                    }
                    TabViewModel item = new TabViewModel
                    {
                        Name = set.Name,
                        Width = launcherSettings.Groups.Max<GroupSet>((Func<GroupSet, int>)(t => t.Width)),
                        Height = launcherSettings.Groups.Max<GroupSet>((Func<GroupSet, int>)(t => t.Height)),
                        TabGroupCollection = observables
                    };
                    this.tabViewCollection.Add(item);
                }
                base.DataContext = this.tabViewCollection;
            }), new object[0]);
        }

        public event Action<SystemTime, Brand> EventNewBrandLauncherView;

        private void newBrandLauncherView(Interop.SystemTime time, Brand brand)
        {
            BrandLauncherView view = new BrandLauncherView(this.time) {
                Top = Application.Current.MainWindow.Top,
                Left = Application.Current.MainWindow.Left + base.Width,
                Height = Application.Current.MainWindow.Height,
                DataContext = brand,
                Title = brand.NameAndFolder,
                ShowActivated = false,
                Owner = this
            };

            _dictBrandLauncherView.Add(brand.BrandId, view);
            view.EventNewBrowserLauncherView += new Action<Interop.SystemTime, BrandProvider>(newBrowserLauncherView);
            view.Closing += new CancelEventHandler(closingViewer<BrandLauncherView>);

            this.currentBrandLauncher = view;
            this.currentBrandLauncher.Show();
        }

        private void newBrowserLauncherView(Interop.SystemTime time, BrandProvider brandProvider)
        {
            _dictBrowserLauncherView.Add(brandProvider.ProviderId, new BrowserLauncherView(brandProvider.Uri, brandProvider.Title, time, brandProvider.Login, brandProvider.Password));
            _dictBrowserLauncherView[brandProvider.ProviderId].Closing += new CancelEventHandler(closingViewer <BrowserLauncherView>);
            _dictBrowserLauncherView[brandProvider.ProviderId].Show();
        }

        private void closingViewer<T>(object obj, CancelEventArgs ev)
        {
            if (typeof(T).Equals(typeof(BrowserLauncherView)) == true)
                _dictBrowserLauncherView.Remove(_dictBrowserLauncherView.FirstOrDefault(x => x.Value == obj).Key);
            else if (typeof(T).Equals(typeof(BrandLauncherView)) == true)
                _dictBrandLauncherView.Remove(_dictBrandLauncherView.FirstOrDefault(x => x.Value == obj).Key);
            else
                ;
        }

        private void button_onClick(Brand brandown)
        {
            LoadUpdates();
            //minimizeMemory();
            if (HaveNewUpdate)
            {
                this.BuildWindow();
                HaveNewUpdate = false;
            }

            string url = "";
            try
            {
                if (brandown != null)
                {
                    if (brandown.MenuWindow)
                    {
                        if (_dictBrandLauncherView.ContainsKey(brandown.BrandId) == false)
                            EventNewBrandLauncherView?.Invoke(this.time, brandown);
                        else
                            _dictBrandLauncherView[brandown.BrandId].Activate();
                    }
                    else if (brandown.Providers.Count > 0)
                    {
                        string loginFromDB = SettingsFactory.GetLoginFromDB(brandown.Providers[0].ProviderId);
                        string pswFromDB = SettingsFactory.GetPswFromDB(brandown.Providers[0].ProviderId);
                        string title = brandown.Providers[0].Title;

                        url = brandown.Providers[0].Uri;
                        if (url.StartsWith("http") || url.StartsWith("https"))
                        {
                            if (_dictBrowserLauncherView.ContainsKey(brandown.Providers[0].ProviderId) == false)
                                newBrowserLauncherView(this.time, brandown.Providers[0]);
                            else
                                _dictBrowserLauncherView[brandown.Providers[0].ProviderId].Activate();
                        }
                        else
                        {
                            url = (url.StartsWith("http") || url.StartsWith("https"))
                                ? url
                                    : Path.Combine(ResourceManager.Root, brandown.NameAndFolder, url);

                            string str6 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                , ResourceManager.Root
                                , brandown.NameAndFolder);

                            using (Process process = new Process {
                                StartInfo = {
                                    UseShellExecute = false
                                    , FileName = url
                                    , CreateNoWindow = true
                                    , Verb = url }
                            })
                            {
                                process.Start();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("button_onClick", url + " | " + exception.Message + " | " + exception.StackTrace);
                MessageBox.Show(exception.Message + url + " | " + exception.StackTrace);
            }

            //Func<CatalogApi.Settings.Brand, bool> predicate = null;
            ////LoadUpdates();
            //minimizeMemory();
            //if (HaveNewUpdate)
            //{
            //    this.BuildWindow();
            //    HaveNewUpdate = false;
            //}
            //Button d = (Button)sender;
            //int brandId = ButtonBehavior.GetBrandId(d);
            //if (this.brandLauncher != null)
            //{
            //    this.brandLauncher.Close();
            //}
            //foreach (GroupSet set in launcherSettings.Groups)
            //{
            //    foreach (CatalogApi.Settings.GroupBox box in set.GroupBoxs)
            //    {
            //        string url = "";
            //        try
            //        {
            //            if (predicate == null)
            //            {
            //                predicate = t => t.BrandId == brandId;
            //            }
            //            CatalogApi.Settings.Brand brand = box.Brands.FirstOrDefault<CatalogApi.Settings.Brand>(predicate);
            //            if (brand != null)
            //            {
            //                if (brand.MenuWindow)
            //                {
            //                    BrandLauncherView view = new BrandLauncherView(this.time)
            //                    {
            //                        Top = Application.Current.MainWindow.Top,
            //                        Left = Application.Current.MainWindow.Left + base.Width,
            //                        Height = Application.Current.MainWindow.Height,
            //                        DataContext = brand,
            //                        Title = brand.NameAndFolder,
            //                        ShowActivated = false,
            //                        Owner = this
            //                    };
            //                    this.brandLauncher = view;
            //                    this.brandLauncher.Show();
            //                }
            //                else if (brand.Providers.Count > 0)
            //                {
            //                    string loginFromDB = SettingsFactory.GetLoginFromDB(brand.Providers[0].ProviderId);
            //                    string pswFromDB = SettingsFactory.GetPswFromDB(brand.Providers[0].ProviderId);
            //                    string title = brand.Providers[0].Title;
            //                    url = brand.Providers[0].Uri;
            //                    if (url.StartsWith("http") || url.StartsWith("https"))
            //                    {
            //                        new BrowserLauncherView(url, title, this.time, loginFromDB, pswFromDB).Show();
            //                    }
            //                    else
            //                    {
            //                        url = (url.StartsWith("http") || url.StartsWith("https")) ? url : Path.Combine(ResourceManager.Root, brand.NameAndFolder, url);
            //                        string str6 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ResourceManager.Root, brand.NameAndFolder);
            //                        using (Process process = new Process { StartInfo = { UseShellExecute = false, FileName = url, CreateNoWindow = true, Verb = url } })
            //                        {
            //                            process.Start();
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception exception)
            //        {
            //            ErrorLogHelper.AddErrorInLog("Button_Click", url + " | " + exception.Message + " | " + exception.StackTrace);
            //            MessageBox.Show(exception.Message + url + " | " + exception.StackTrace);
            //        }
            //    }
            //}
        }

        private void CheckForUpdateAndStartProcess()
        {
            try
            {
                Logging(string.Format("::CheckForUpdateAndStartProcess() - вход..."));

                if (Directory.Exists("Temp"))
                {
                    IoHelper.DirectoryClear("Temp");
                }

                AccountManager.Account = RequestHelper.Client.GetUnoccupiedAccount();

                launcherSettings = new SettingsFactory(categoryEventHandler).DownloadSettings(false, true);
                this.BuildWindow();

                if (System.IO.File.Exists("Version.json")) {
                    this.RefreshTitle(new Version(JsonConvert.DeserializeObject<VersionEntity>(FileHelper.OpenFile("Version.json")).Version).ToString());
                } else
                    ;

                this.SetWindowVisibility(Visibility.Visible);

                Logging(string.Format("::CheckForUpdateAndStartProcess() - успех..."));
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("CheckForUpdateAndStartProcess()", exception.Message + " | " + exception.StackTrace);
                MessageBox.Show(exception.Message + " | " + exception.StackTrace);

                Application.Current.Shutdown(-1);
            }
        }

        private static void FreeOccupiedAccount()
        {
            try {
                if (!((AccountManager.Account == null)
                    || (string.IsNullOrEmpty(AccountManager.Account.Name) == true))) {
                    RequestHelper.Client.FreeOccupiedAccount(AccountManager.Account.Name);
                } else
                    ;
            } catch (Exception e) {
                ErrorLogHelper.AddErrorInLog(string.Format(@"MainWindow::FreeOccupiedAccount () - ...")
                    , string.Format("{0} | {1}", e.Message, e.StackTrace));
            }
        }

        private void InitializeSettings()
        {
            Logging(string.Format("::InitializeSettings() - вход..."));

            if (RequestHelper.Client == null)
            {
                RequestHelper.Client = new RequestProcessorClient();
            }
            #region  Khryapin 2017/04/29
            if ((Application.Current as App).IsRelease == false) {
                var endpoint = RequestHelper.Client.Endpoint;
                Logging(string.Format("EndPoint=[Name={0}, ListenUri={1}, ListenUriMode={2}, Contract.Name={3}, Binding.Name={4}, Address.Uri={5}]"
                    , endpoint.Name
                    , endpoint.ListenUri.ToString()
                    , endpoint.ListenUriMode.ToString()
                    , endpoint.Contract.Name
                    , endpoint.Binding.Name
                    , endpoint.Address.Uri));

                var clientCredentials = RequestHelper.Client.ClientCredentials;
                if (!(RequestHelper.Client.ClientCredentials == null))
                    Logging(string.Format("ClientCredentials=[Certificate.SerialNumber={0}, UserName={1}, HttpDigest.UserName={2}, HttpDigest.Password={3}, Windows.AllowedImpersonationLevel={4}]"
                        , (clientCredentials.ClientCertificate.Certificate == null) ? string.Empty : clientCredentials.ClientCertificate.Certificate.SerialNumber
                        , clientCredentials.UserName.UserName
                        , clientCredentials.HttpDigest.ClientCredential.UserName
                        , clientCredentials.HttpDigest.ClientCredential.Password
                        , clientCredentials.Windows.AllowedImpersonationLevel
                    ));
                else
                    ;

                Logging(string.Format("RequestHelper.Client.State={0}", RequestHelper.Client.State.ToString()));
            } else
                ;

            #endregion
            this.SetWindowStartupLocation();
            this.SetWindowVisibility(Visibility.Hidden);

            FileHelper.CreateFileIfNotExist("Version.json");
            FileHelper.CreateDirectoryIfNotExist("Temp");
            InteropHelper.GetSystemTime(ref this.time);

            RemoteCertificateValidationCallback delegateCertificateValidationAlwaysTrust = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
            //ServicePointManager.ServerCertificateValidationCallback =
            //    (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback
            //        , delegateCertificateValidationAlwaysTrust);
            ServicePointManager.ServerCertificateValidationCallback +=
                delegateCertificateValidationAlwaysTrust;

            Logging(string.Format("::InitializeSettings() - успех..."));
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if ((imageData == null) || (imageData.Length == 0))
            {
                return null;
            }
            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                stream.Position = 0L;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private static void LoadUpdates()
        {
            Exception exception;
            try
            {
                using (AvtoritetEntities entities = new AvtoritetEntities())
                {
                    //if (!(entities.Database.Connection.State == ConnectionState.Closed))
                    //{
                        int? settingVersion = entities.SettingUpdate.FirstOrDefault().SettingVersion;
                        Version version = new Version(entities.VersionLog.FirstOrDefault().Value)
                            , version2 = new Version(JsonConvert.DeserializeObject<VersionEntity>(FileHelper.OpenFile("Version.json")).Version);
                        if (version > version2) {
                            try {
                            } catch (Exception exception1) {
                                exception = exception1;
                                ErrorLogHelper.AddErrorInLog("LoadUpdates()", exception.Message + " | " + exception.StackTrace);
                            }
                        }
                        if (!System.IO.File.Exists("settingver.txt")) {
                            System.IO.File.WriteAllText("settingver.txt", "1");
                        }
                        int num = int.Parse(System.IO.File.ReadAllText("settingver.txt"));
                        int? nullable2 = settingVersion;
                        int num2 = num;
                        if ((nullable2.GetValueOrDefault() > num2) && nullable2.HasValue) {
                            launcherSettings = new SettingsFactory(categoryEventHandler).DownloadSettings(true, true);
                            System.IO.File.WriteAllText("settingver.txt", settingVersion.ToString());
                            HaveNewUpdate = true;
                        }
                    //}
                    //else
                    //ErrorLogHelper.AddErrorInLog("Обновление приложения - LoadUpdates() - ...", string.Format("Состояние БД={0}", entities.Database.Connection.State.ToString()));
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLogHelper.AddErrorInLog("Обновление приложения - LoadUpdates()", exception.Message + " | " + exception.StackTrace);
                MessageBox.Show(exception.Message + " | " + exception.StackTrace);
            }
        }

        private static void minimizeMemory()
        {
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            Int32 max = -1;
            Int32 min = -1;
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, ((IntPtr)max), ((IntPtr)min));
        }

        private void NewsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RefreshTitle(string version)
        {
            base.Title = string.Empty;
            base.Title = "Авторитет - " + version;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems == null || e.AddedItems.Count <= 0) return;

                var viewModel = (TabViewModel)e.AddedItems[0];

                NewsBox.Visibility = viewModel.Name == "Новости"
                    ? NewsBox.Visibility = Visibility.Visible
                    : Visibility.Collapsed;
                if (NewsBox.Visibility != Visibility.Visible) return;

                News.Clear();

                Task.Factory.StartNew(async () =>
                 {
                    try
                    {
                        var newsLogs = await ConfigurationManager.AppSettings["NewsApi"].GetJsonAsync<NewsModel[]>();

                        foreach (var newsModel in newsLogs.OrderByDescending(x => x.PostTime))
                        {
                            var id = newsModel.Id;

                            if (News.Select(x => x.Id).Contains(id)) continue;

                            var title = newsModel.Title;
                            var message = newsModel.Message;
                            var postTime = newsModel.PostTime;

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    News.Add(new NewsModel
                                    {
                                        Id = id,
                                        Title = title,
                                        Message = message,
                                        PostTime = postTime
                                    });
                                }
                                catch (Exception ex)
                                {
                                    //todo: Add Log To Sql
                                    MessageBox.Show(ex.Message + " || " + ex.StackTrace);
                                }
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        //todo: Add Log To Sql
                        MessageBox.Show(ex.Message + " || " + ex.StackTrace);
                    }
                });
            }
            catch (Exception ex)
            {
                //todo: Add Log To Sql
                MessageBox.Show(ex.Message + " || " + ex.StackTrace);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, IntPtr minimumWorkingSetSize, IntPtr maximumWorkingSetSize);

        private void SetWindowStartupLocation()
        {
            base.Top = 50.0;
            base.Left = 50.0;
        }

        private void SetWindowVisibility(Visibility visibility)
        {
            base.Visibility = visibility;
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private void StartNewEventSession()
        {
            try
            {
                Logging(string.Format("::StartNewEventSession() - вход..."));

                base.Closing += delegate(object sender, System.ComponentModel.CancelEventArgs args)
                {
                    if ((App.IsAssemblyRelease(Assembly.GetAssembly(this.GetType())) == false)
                        || (this.isNormalShutdownMode == true)) {
                        args.Cancel = false;

                        foreach (BrowserLauncherView view in _dictBrowserLauncherView.Values) {
                            view.Closing -= new CancelEventHandler(closingViewer<BrowserLauncherView>);
                            view.Close();
                        }
                        _dictBrowserLauncherView.Clear();

                        foreach (BrandLauncherView view in _dictBrandLauncherView.Values) {
                            view.Closing -= new CancelEventHandler(closingViewer<BrandLauncherView>);
                            view.Close();
                        }
                        _dictBrandLauncherView.Clear();

                        MainWindow.FreeOccupiedAccount();
                    } else {
                        args.Cancel = true;

                        base.WindowState = WindowState.Minimized;
                        base.ShowInTaskbar = true;

                        if (this.currentBrandLauncher != null) {
                            this.currentBrandLauncher.Visibility = Visibility.Hidden;
                        } else
                            ;
                    }
                };

                base.StateChanged += delegate //(object sender, System.EventArgs args)
                {
                    try
                    {
                        if ((this.currentBrandLauncher != null)
                            && ((this.currentBrandLauncher == null)
                                || (this.currentBrandLauncher.Visibility == Visibility.Hidden))
                            )
                        {
                            try
                            {
                                this.currentBrandLauncher.Visibility = Visibility.Visible;
                            }
                            catch (Exception)
                            {
                            }

                            MainWindow.ShowWindow(new WindowInteropHelper(this.currentBrandLauncher).Handle, SwRestore);
                        }
                    }
                    catch (System.Exception ex2)
                    {
                        ErrorLogHelper.AddErrorInLog("StartNewEventSession()", ex2.Message + " | " + ex2.StackTrace);
                        MessageBox.Show(ex2.Message + " | " + ex2.StackTrace);
                    }
                };

                base.LocationChanged += delegate //(object sender, System.EventArgs args)
                {
                    if (this.DownFlag)
                    {
                        this.currentBrandLauncher.Top = base.Top + base.Height;
                        this.currentBrandLauncher.Left = base.Left + this.LeftLength;
                    }

                    if (this.TopFlag)
                    {
                        this.currentBrandLauncher.Top = base.Top - this.currentBrandLauncher.Height;
                        this.currentBrandLauncher.Left = base.Left + this.LeftLength;
                    }

                    if (this.LeftFlag)
                    {
                        this.currentBrandLauncher.Left = base.Left - this.currentBrandLauncher.Width;
                        this.currentBrandLauncher.Top = base.Top + this.TopLength;
                    }

                    if (this.RightFlag)
                    {
                        this.currentBrandLauncher.Left = base.Left + base.Width;
                        this.currentBrandLauncher.Top = base.Top + this.TopLength;
                    }
                };

                Microsoft.Win32.SystemEvents.SessionEnding += delegate(object sender, Microsoft.Win32.SessionEndingEventArgs args)
                {
                    Microsoft.Win32.SessionEndReasons arg_07_0 = args.Reason;
                    MainWindow.FreeOccupiedAccount();
                };

                Logging(string.Format("::StartNewEventSession() - успех..."));
            }
            catch (System.Exception ex)
            {
                ErrorLogHelper.AddErrorInLog("StartNewEventSession()", ex.Message + " | " + ex.StackTrace);
                MessageBox.Show(ex.Message + " | " + ex.StackTrace);
            }
        }

        private static void TimerCallback(object threadIdArg)
        {
            LoadUpdates();
            minimizeMemory();
        }

        public ObservableCollection<NewsModel> News
        {
            get
            {
                return this.news;
            }
            set
            {
                this.news = value;
                this.OnPropertyChanged("News");
            }
        }

        public static void Logging(Exception e)
        {
            Logging(string.Format("{0} / {1}"
                    , e.Message
                    , e.StackTrace
            ));
        }

        public static void Logging(string mes)
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            FileInfo appFileInfo = new FileInfo(location.AbsolutePath);

            using (FileStream fileStream = new FileStream(string.Format("{0}.log", Path.GetFileNameWithoutExtension(appFileInfo.FullName)), FileMode.Append, FileAccess.Write)) {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {
                    streamWriter.WriteLine(string.Format("[{1:o}]{0}{2}"
                        , Environment.NewLine
                        , DateTime.Now
                        , mes));
                }
            }
        }
    }
}
