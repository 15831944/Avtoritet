﻿using CatalogApi.Settings;
using NewLauncher;
using NewLauncher.Entities;
using NewLauncher.Factory;
using NewLauncher.Helper;
using NewLauncher.Interop;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;


namespace NewLauncher.View
{
    /// <summary>
    /// Логика взаимодействия для BrandLauncherView.xaml
    /// </summary>
    public partial class BrandLauncherView : Window
    {
        private Brand brand = new Brand();
        private readonly ObservableCollection<ButtonModel> categories = new ObservableCollection<ButtonModel>();
        private readonly SystemTime time;


        public BrandLauncherView(SystemTime starTime)
        {
            this.InitializeComponent();
            this.time = starTime;
            base.DataContext = this.categories;
            base.ShowInTaskbar = false;
            this.CategoriesList.ItemsSource = this.categories;
            this.StartNewEventSession();
        }

        private void AddLaunchButton(Brand newBrand, BrandProvider provider)
        {
            ButtonModel item = CreateLaunchButton(newBrand, provider);
            item.Click += new RoutedEventHandler(this.InitButtonClick);
            this.categories.Add(item);
        }

        private static ButtonModel CreateLaunchButton(Brand brand, BrandProvider provider)
        {
            string uri = (provider.Uri.StartsWith("http") || provider.Uri.StartsWith("https")) ? provider.Uri : Path.Combine(ResourceManager.Root, brand.NameAndFolder, provider.Uri);
            try
            {
                if (((!provider.Uri.StartsWith("http") && !provider.Uri.StartsWith("https")) && ((provider.Uri.IndexOf(@"\") >= 0) & (provider.Uri.ToLower().IndexOf(".exe") >= 0))) && File.Exists(provider.Uri))
                {
                    uri = provider.Uri;
                }
            }
            catch
            {
            }
            return new ButtonModel { Height = 40.0, HorizontalContentAlignment = HorizontalAlignment.Center, Margin = new Thickness(0.0, 0.0, 0.0, 0.0), Content = provider.Title, DataContext = uri, Login = provider.Login, Password = provider.Password, ButtonStyle = brand.ButtonStyle, ProviderId = provider.ProviderId };
        }

        private void InitButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = ((ButtonModel)sender).DataContext.ToString();
                if (url.StartsWith("http") || url.StartsWith("https"))
                {
                    if (url.Contains("imtportal.gm"))
                    {
                        StartSeparateProcess(url);
                    }
                    else
                    {
                        string loginFromDB = SettingsFactory.GetLoginFromDB(((ButtonModel)sender).ProviderId);
                        string pswFromDB = SettingsFactory.GetPswFromDB(((ButtonModel)sender).ProviderId);
                        string content = ((ButtonModel)sender).Content;
                        new BrowserLauncherView(url, content, this.time, loginFromDB, pswFromDB).Show();
                    }
                }
                else
                {
                    string str6 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ResourceManager.Root, this.brand.NameAndFolder);
                    using (Process process = new Process { StartInfo = { UseShellExecute = false, FileName = url, CreateNoWindow = true, Verb = url } })
                    {
                        process.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("InitButtonClick", exception.Message + " | " + exception.StackTrace);
                MessageBox.Show(exception.Message + " | " + exception.StackTrace);
                new ReportWindow().ShowReportWindow(exception);
            }
        }

        private void OnActivated(object sender, EventArgs eventArgs)
        {
            try
            {
                base.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/FavActive.ico"));
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnActivated", exception.Message + " | " + exception.StackTrace);
                new ReportWindow().ShowReportWindow(exception);
            }
        }

        private void OnDeactivated(object sender, EventArgs eventArgs)
        {
            try
            {
                base.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/FavInactive.ico"));
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnDeactivated", exception.Message + " | " + exception.StackTrace);
                new ReportWindow().ShowReportWindow(exception);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                this.brand = (Brand)base.DataContext;
                if (this.brand.IconPath.ToLower().Contains("gaz"))
                {
                    this.Alert.Visibility = Visibility.Visible;
                }
                base.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/FavInactive.ico"));
                this.categories.Clear();
                foreach (BrandProvider provider in this.brand.Providers)
                {
                    this.AddLaunchButton(this.brand, provider);
                }
                this.ImageHostControl.Source = UiHelper.LoadImage(this.brand.IconPath2Img);
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnLoaded", exception.Message + " | " + exception.StackTrace);
                Debug.WriteLine("[{0}] {1} / {2}", new object[] { DateTime.Now, exception.Message, exception.StackTrace });
            }
        }

        private void OnLocationChanged(object sender, EventArgs eventArgs)
        {
            MainWindow owner = base.Owner as MainWindow;
            if (owner != null)
            {
                if ((base.Top < ((owner.Top + owner.Height) + 20.0)) && (base.Top > ((owner.Top + owner.Height) - 20.0)))
                {
                    base.Top = owner.Top + owner.Height;
                    if (!owner.DownFlag)
                    {
                        owner.LeftLength = base.Left - owner.Left;
                    }
                    owner.DownFlag = true;
                }
                else
                {
                    owner.DownFlag = false;
                }
                if (((base.Top + base.Height) < (owner.Top + 20.0)) && ((base.Top + base.Height) > (owner.Top - 20.0)))
                {
                    base.Top = owner.Top - base.Height;
                    if (!owner.TopFlag)
                    {
                        owner.LeftLength = base.Left - owner.Left;
                    }
                    owner.TopFlag = true;
                }
                else
                {
                    owner.TopFlag = false;
                }
                if (((base.Left + base.Width) < (owner.Left + 20.0)) && ((base.Left + base.Width) > (owner.Left - 20.0)))
                {
                    base.Left = owner.Left - base.Width;
                    if (!owner.LeftFlag)
                    {
                        owner.TopLength = base.Top - owner.Top;
                    }
                    owner.LeftFlag = true;
                }
                else
                {
                    owner.LeftFlag = false;
                }
                if ((base.Left < ((owner.Left + owner.Width) + 20.0)) && (base.Left > ((owner.Left + owner.Width) - 20.0)))
                {
                    base.Left = owner.Left + owner.Width;
                    if (!owner.RightFlag)
                    {
                        owner.TopLength = base.Top - owner.Top;
                    }
                    owner.RightFlag = true;
                }
                else
                {
                    owner.RightFlag = false;
                }
            }
        }

        private void StartNewEventSession()
        {
            base.Loaded += new RoutedEventHandler(this.OnLoaded);
            base.Activated += new EventHandler(this.OnActivated);
            base.Deactivated += new EventHandler(this.OnDeactivated);
            base.LocationChanged += new EventHandler(this.OnLocationChanged);
        }

        private static void StartSeparateProcess(string url)
        {
            FileStream stream;
            StreamWriter writer;
            RequestHelper.Client.OpenSession(url, true);
            string cookies = RequestHelper.Client.GetCookies(url);
            if (url.Contains("opel"))
            {
                using (stream = new FileStream("Session_Opel.txt", FileMode.Create, FileAccess.Write))
                {
                    using (writer = new StreamWriter(stream))
                    {
                        writer.Write(cookies);
                    }
                }
            }
            if (url.Contains("chevrolet"))
            {
                using (stream = new FileStream("Session_Chevrolet.txt", FileMode.Create, FileAccess.Write))
                {
                    using (writer = new StreamWriter(stream))
                    {
                        writer.Write(cookies);
                    }
                }
            }
            using (Process process = new Process { StartInfo = { FileName = "BrowserExtension.exe", Arguments = url, CreateNoWindow = true, UseShellExecute = false } })
            {
                process.Start();
            }
        }

    }
}
