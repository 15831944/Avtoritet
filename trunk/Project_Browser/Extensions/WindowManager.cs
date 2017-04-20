using System;
using System.Windows;

namespace BrowserExtension.Extensions
{
	public class WindowManager
	{
		public void Open(Uri url)
		{
			ExtendedWebBrowser extendedWebBrowser = this.New(false, null);
			extendedWebBrowser.Navigate(url);
		}

		public ExtendedWebBrowser New()
		{
			return this.New(true, null);
		}

		public ExtendedWebBrowser New(bool navigateHome, string title = null)
		{
			BrowserWindow browserWindow = new BrowserWindow
			{
				IeHost = 
				{
					Visibility = Visibility.Visible
				},
				Title = title,
				Width = 800.0,
				Height = 600.0,
				WindowState = WindowState.Normal,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			browserWindow.Show();
			return browserWindow.IeWeb;
		}
	}
}
