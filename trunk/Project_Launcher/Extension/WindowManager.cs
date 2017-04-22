namespace NewLauncher.Extension
{
    using NewLauncher.View;
    using System;
    using System.Windows;

    public class WindowManager
    {
        public ExtendedWebBrowser New()
        {
            return this.New(true);
        }

        public ExtendedWebBrowser New(bool navigateHome)
        {
            BrowserDialogView view = new BrowserDialogView {
                IeHost = { Visibility = Visibility.Visible },
                GeckoHost = { Visibility = Visibility.Collapsed },
                Width = 800.0,
                Height = 600.0,
                WindowState = WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            view.Show();
            return view.WebBrowser;
        }

        public ExtendedWebBrowser NewMaxSizeWindow(bool navigateHome)
        {
            BrowserDialogView view = new BrowserDialogView {
                IeHost = { Visibility = Visibility.Visible },
                GeckoHost = { Visibility = Visibility.Collapsed },
                Width = 800.0,
                Height = 600.0,
                WindowState = WindowState.Maximized,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            view.Show();
            return view.WebBrowser;
        }

        public void Open(Uri url)
        {
            this.New(false).Navigate(url);
        }
    }
}

